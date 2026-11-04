global using GroupSettingData =
    (System.Collections.Generic.Dictionary<(System.Guid Identifier, string? Name), byte> Options, bool DisableAllUnknown);
global using ModObjectIdentifier = (System.Guid Identifier, string? Name);
global using SettingPresetData = (System.Collections.Generic.Dictionary<(System.Guid Identifier, string? Name),
        (System.Collections.Generic.Dictionary<(System.Guid Identifier, string? Name), byte> Options, bool DisableAllUnknown)> Settings, int
    _priority, short Version, bool _hasPriority, byte _state);
using System.Buffers;
using System.Text.Json;
using ImSharp;
using Luna;
using Penumbra.Api.Preset;

namespace Penumbra.GameData.Gui;

[Flags]
public enum PenumbraOptionDisplay
{
    HideIds       = 0x01,
    HideAddOption = 0x02,
    HideResolve   = 0x04,
    HideIndex     = 0x08,
}

public static class PresetExtensions
{
    public delegate IReadOnlyList<(ModObjectIdentifier, bool)>? GetGroupDataDelegate(in ModObjectIdentifier groupIdentifier,
        out string? name, out bool single);

    private static readonly EnumCombo<ModState> ModStateCombo =
        new(ModStateExtensions.ToNameU8, ModStateExtensions.ToName, ModStateExtensions.Tooltip);

    private static readonly EnumCombo<OptionState> OptionStateCombo =
        new(OptionStateExtensions.ToNameU8, OptionStateExtensions.ToName, OptionStateExtensions.Tooltip);

    extension(PenumbraOptionDisplay)
    {
        public static bool DrawConfigButton(PenumbraOptionDisplay current, out PenumbraOptionDisplay result)
        {
            result = current;
            if (ImEx.Icon.Button(LunaStyle.ConfigIcon, "Edit the information displayed here."u8) || Im.Item.RightClicked())
                Im.Popup.Open("##oc"u8);

            Im.Window.SetNextSize(ImEx.ScaledVectorX(250, 4 * Im.Style.FrameHeightWithSpacing + 3 * Im.Style.TextHeightWithSpacing + Im.Style.WindowPadding.Y));
            using var popup = Im.Popup.Begin("##oc"u8);
            if (!popup)
                return false;

            var ret = Im.Checkbox("Hide GUID Input and Buttons"u8, ref result, PenumbraOptionDisplay.HideIds);
            ret |= Im.Checkbox("Hide Resolved Text and Buttons"u8,   ref result, PenumbraOptionDisplay.HideResolve);
            ret |= Im.Checkbox("Hide Object Indices"u8,              ref result, PenumbraOptionDisplay.HideIndex);
            ret |= Im.Checkbox("Hide Sections to Add New Objects"u8, ref result, PenumbraOptionDisplay.HideAddOption);
            Im.TextWrapped("If GUIDs, Resolved Text and Indices are hidden, Options are displayed in a single line."u8);
            return ret;
        }
    }

    extension(SettingPreset preset)
    {
        public void DrawIdentifier(Vector2 size)
        {
            using (Im.Font.PushMono())
            {
                if (ImEx.Button($"{preset.Identifier}", size, "Click to copy to clipboard."u8))
                    Im.Clipboard.Set($"{preset.Identifier}");
            }

            Im.Line.SameInner();
            ImEx.TextFrameAligned("Identifier"u8);
        }

        public bool DrawName(Vector2 size, out string newName)
        {
            Im.Item.SetNextWidth(size.X);
            return ImEx.InputOnDeactivation.Text("Name"u8, preset.Name, out newName, "Name..."u8);
        }

        public bool DrawEditTime(Vector2 size, out DateTimeOffset newTime)
        {
            using var id = Im.Id.Push(1);
            ImEx.TextFramed($"{preset.LastEdit.LocalDateTime:yyyy/MM/dd HH:mm}",
                size.AddX(-Im.Style.FrameHeight - Im.Style.ItemInnerSpacing.X));
            Im.Line.SameInner();
            bool ret;

            if (ImEx.Icon.Button(LunaStyle.RefreshIcon, "Reset the last edit date to now."u8, !LunaStyle.Modifier.Misclick))
            {
                newTime = DateTimeOffset.UtcNow;
                ret     = true;
            }
            else
            {
                newTime = preset.LastEdit;
                ret     = false;
            }

            LunaStyle.Modifier.Misclick.TooltipLineBreak("update"u8);
            Im.Line.SameInner();
            ImEx.TextFrameAligned("Last Edit"u8);
            return ret;
        }

        public bool DrawApplicationTime(Vector2 size, out DateTimeOffset newTime)
        {
            using var id = Im.Id.Push(1);
            ImEx.TextFramed($"{preset.LastApplication.LocalDateTime:yyyy/MM/dd HH:mm}",
                size.AddX(-Im.Style.FrameHeight - Im.Style.ItemInnerSpacing.X));
            Im.Line.SameInner();
            bool ret;

            if (ImEx.Icon.Button(LunaStyle.RefreshIcon, "Reset the last application date to now."u8, !LunaStyle.Modifier.Misclick))
            {
                newTime = DateTimeOffset.UtcNow;
                ret     = true;
            }
            else
            {
                newTime = preset.LastApplication;
                ret     = false;
            }

            LunaStyle.Modifier.Misclick.TooltipLineBreak("update"u8);
            Im.Line.SameInner();
            ImEx.TextFrameAligned("Last Application"u8);
            return ret;
        }
    }


    extension(in SettingPresetData preset)
    {
        public bool DrawState(Vector2 size, out ModState newState)
            => ModStateCombo.Draw("Mod State"u8, preset.State,
                "Configure the state to set the mod to. Note that when inheriting, no further changes are applied."u8, size.X, out newState);

        public bool DrawPriority(Vector2 size, out int? newPriority)
        {
            var ret = false;
            newPriority = null;
            if (Im.Checkbox("##prioIgnore"u8, preset._hasPriority))
            {
                ret         = true;
                newPriority = preset._hasPriority ? null : 0;
            }

            Im.Tooltip.OnHover("Disable this to keep the priority as is."u8);
            Im.Line.SameInner();
            using (Im.Disabled(!preset._hasPriority))
            {
                Im.Item.SetNextWidth(size.X - Im.Style.FrameHeight - Im.Style.ItemInnerSpacing.X);
                if (ImEx.InputOnDeactivation.Scalar("##prio"u8, preset._priority, out var newValue))
                {
                    ret         = true;
                    newPriority = newValue;
                }
            }

            Im.Line.SameInner();
            ImEx.TextFrameAligned("Mod Priority"u8);

            return ret;
        }

        public static bool DrawGroup(Vector2 size, int index, in ModObjectIdentifier group, in ModObjectIdentifier? resolvedGroup,
            bool disableUnknown, out ModObjectIdentifier? newGroup, out bool? newDisableUnknown, PenumbraOptionDisplay options = default)
        {
            newGroup          = group;
            newDisableUnknown = null;
            var ret = false;

            var showIds     = !options.HasFlag(PenumbraOptionDisplay.HideIds);
            var showResolve = !options.HasFlag(PenumbraOptionDisplay.HideResolve);
            var showIndex   = !options.HasFlag(PenumbraOptionDisplay.HideIndex);
            if (showIds)
                ret |= DrawGuidInput(size, index, group, ref newGroup, showIndex, true);

            ret |= DrawNameInput(size, group, ref newGroup, 2 + (showIds ? 1 : 0) + (showResolve ? 1 : 0));

            if (showIds)
            {
                Im.Line.SameInner();
                if (ImEx.Icon.Button(LunaStyle.TagsMarker, "Remove the name from this group reference and keep only the GUID."u8,
                        group.Identifier == Guid.Empty)
                 && group.Name is not null)
                {
                    ret      = true;
                    newGroup = group with { Name = null };
                }
            }

            if (showResolve)
            {
                Im.Line.SameInner();
                if (ImEx.Icon.Button(LunaStyle.RefreshIcon, "Update this identifier to the currently resolved group."u8,
                        !resolvedGroup.HasValue))
                {
                    ret      = true;
                    newGroup = resolvedGroup!.Value;
                }
            }

            Im.Line.SameInner();
            if (ImEx.Icon.Button(LunaStyle.DeleteIcon, "Delete this group reference entirely from the preset."u8,
                    !LunaStyle.Modifier.Destructive))
            {
                ret      = true;
                newGroup = null;
            }

            Im.Line.SameInner();
            if (Im.Checkbox("##unk"u8, disableUnknown))
            {
                newDisableUnknown = !disableUnknown;
                ret               = true;
            }

            Im.Tooltip.OnHover("Disable all unknown options in this group."u8);

            if (!showIds)
                IndexText(index, group, showIndex, true);
            ResolvedText(showResolve, resolvedGroup);
            return ret;
        }

        private static void IndexText(int index, in ModObjectIdentifier @object, bool showIndex, bool group)
        {
            if (!showIndex)
                return;

            Im.Line.SameInner();
            ImEx.TextFrameAligned(group ? $"Group #{index + 1}" : $"Option #{index + 1}");
            if (@object.Identifier != Guid.Empty)
                Im.Tooltip.OnHover($"{@object.Identifier}");
        }

        private static void ResolvedText(bool showResolve, ModObjectIdentifier? resolved)
        {
            if (!showResolve)
                return;

            Im.Line.SameInner();
            ImEx.TextFrameAligned("(Resolves to "u8);
            Im.Line.NoSpacing();
            Im.Text(resolved?.Name ?? "Nothing", resolved is null ? LunaStyle.WarningForeground : LunaStyle.SuccessForeground);
            Im.Line.NoSpacing();
            Im.Text(")"u8);
        }

        public bool DrawAddGroup(Vector2 size, ref Guid guidInput, ref string nameInput, out ModObjectIdentifier identifier,
            ModObjectIdentifier? resolved, PenumbraOptionDisplay options = default)
        {
            if (options.HasFlag(PenumbraOptionDisplay.HideAddOption))
            {
                identifier = default;
                return false;
            }

            using var id = Im.Id.Push("newg"u8);
            if (ImEx.GuidInput("##id"u8, guidInput == Guid.Empty ? StringU8.Empty : $"{guidInput}", out var newGuid, size.X))
                guidInput = newGuid;

            Im.Line.SameInner();
            ImEx.TextFrameAligned("New Group"u8);

            Im.Item.SetNextWidth(size.X - Im.Style.ItemInnerSpacing.X - Im.Style.FrameHeight);
            Im.Input.Text("##name"u8, ref nameInput, "Name..."u8);

            identifier = new ModObjectIdentifier(guidInput, nameInput);
            var valid     = !identifier.IsEmpty;
            var contained = valid && preset.Settings.ContainsKey(identifier);
            var tt = contained ? "An equivalent group reference is already contained in the preset."u8 :
                valid          ? "Add this group reference to the preset."u8 :
                                 "Please enter a valid GUID and/or name to add a group reference to the preset."u8;
            Im.Line.SameInner();
            var ret = ImEx.Icon.Button(LunaStyle.AddObjectIcon, tt, contained || !valid);
            ResolvedText(!options.HasFlag(PenumbraOptionDisplay.HideResolve), resolved);
            if (ret)
            {
                guidInput = Guid.Empty;
                nameInput = string.Empty;
            }

            return ret;
        }

        public static bool DrawOption(Vector2 size, int index, in ModObjectIdentifier option, in ModObjectIdentifier? resolvedOption,
            in OptionState state, out ModObjectIdentifier? newOption, out OptionState? newState, PenumbraOptionDisplay options = default)
        {
            newOption = option;
            newState  = null;
            var ret = false;

            var showIds     = !options.HasFlag(PenumbraOptionDisplay.HideIds);
            var showResolve = !options.HasFlag(PenumbraOptionDisplay.HideResolve);
            var showIndex   = !options.HasFlag(PenumbraOptionDisplay.HideIndex);
            if (showIds)
                ret |= DrawGuidInput(size, index, option, ref newOption, showIndex, false);

            ret |= DrawNameInput(size, option, ref newOption, 1 + (showIds ? 1 : 0) + (showResolve ? 1 : 0));

            if (showIds)
            {
                Im.Line.SameInner();
                if (ImEx.Icon.Button(LunaStyle.TagsMarker, "Remove the name from this option reference and keep only the GUID."u8,
                        option.Identifier == Guid.Empty)
                 && option.Name is not null)
                {
                    ret       = true;
                    newOption = option with { Name = null };
                }
            }

            if (showResolve)
            {
                Im.Line.SameInner();
                if (ImEx.Icon.Button(LunaStyle.RefreshIcon, "Update this identifier to the currently resolved option."u8,
                        !resolvedOption.HasValue))
                {
                    ret       = true;
                    newOption = resolvedOption!.Value;
                }
            }

            Im.Line.SameInner();
            if (ImEx.Icon.Button(LunaStyle.DeleteIcon, "Delete this option reference entirely from the preset."u8,
                    !LunaStyle.Modifier.Destructive))
            {
                ret       = true;
                newOption = null;
            }

            if (!showIds)
                IndexText(index, option, showIndex, false);
            else
                ResolvedText(showResolve, resolvedOption);

            if (!showIds && !showResolve && !showIndex)
            {
                Im.Line.SameInner();
                size.X = 90 * Im.Style.GlobalScale;
            }

            if (OptionStateCombo.Draw("##state"u8, state.ToNameU8(), "The state to set this option to inside this group."u8,
                    size.X, out var tmpState))
            {
                newState = tmpState.Item;
                ret      = true;
            }

            if (!showIds)
                ResolvedText(showResolve, resolvedOption);

            return ret;
        }

        public static bool DrawAddOption(Vector2 size, in GroupSettingData group, ref Guid guidInput, ref string nameInput,
            out ModObjectIdentifier identifier, ModObjectIdentifier? resolved, PenumbraOptionDisplay options = default)
        {
            if (options.HasFlag(PenumbraOptionDisplay.HideAddOption))
            {
                identifier = default;
                return false;
            }

            using var id = Im.Id.Push("newo"u8);
            if (ImEx.GuidInput("##id"u8, guidInput == Guid.Empty ? StringU8.Empty : $"{guidInput}", out var newGuid, size.X))
                guidInput = newGuid;

            Im.Line.SameInner();
            ImEx.TextFrameAligned("New Option"u8);

            Im.Item.SetNextWidth(size.X - Im.Style.ItemInnerSpacing.X - Im.Style.FrameHeight);
            Im.Input.Text("##name"u8, ref nameInput, "Name..."u8);

            identifier = new ModObjectIdentifier(guidInput, nameInput);
            var valid     = !identifier.IsEmpty;
            var contained = valid && group.Options.ContainsKey(identifier);
            var tt = contained ? "An equivalent option reference is already contained in the current group."u8 :
                valid          ? "Add this option reference to the preset."u8 :
                                 "Please enter a valid GUID and/or name to add an option reference to the current group."u8;
            Im.Line.SameInner();
            var ret = ImEx.Icon.Button(LunaStyle.AddObjectIcon, tt, contained || !valid);
            ResolvedText(!options.HasFlag(PenumbraOptionDisplay.HideResolve), resolved);
            if (ret)
            {
                guidInput = Guid.Empty;
                nameInput = string.Empty;
            }

            return ret;
        }

        private static bool DrawGuidInput(Vector2 size, int index, in ModObjectIdentifier @object, ref ModObjectIdentifier? newObject,
            bool showIndex,
            bool group)
        {
            var ret = false;
            if (ImEx.GuidInput("##id"u8, @object.Identifier == Guid.Empty ? StringU8.Empty : $"{@object.Identifier}", out var newGuid,
                    size.X - Im.Style.ItemInnerSpacing.X - Im.Style.FrameHeight))
            {
                ret       = true;
                newObject = @object with { Identifier = newGuid };
            }

            Im.Line.SameInner();
            if (ImEx.Icon.Button(LunaStyle.ResetIcon,
                    group
                        ? "Make this group reference generic by removing the GUID and keeping only the name."u8
                        : "Make this option reference generic by removing the GUID and keeping only the name."u8,
                    string.IsNullOrEmpty(@object.Name) || @object.Identifier == Guid.Empty))
            {
                ret       = true;
                newObject = @object with { Identifier = Guid.Empty };
            }

            IndexText(index, default, showIndex, group);
            return ret;
        }

        private static bool DrawNameInput(Vector2 size, in ModObjectIdentifier @object, ref ModObjectIdentifier? newObject, int buttonCount)
        {
            Im.Item.SetNextWidth(size.X - buttonCount * (Im.Style.ItemInnerSpacing.X + Im.Style.FrameHeight));
            if (!ImEx.InputOnDeactivation.Text("##name"u8, @object.Name ?? string.Empty, out string newName, "Name..."u8))
                return false;

            newObject = @object with { Name = newName };
            return true;
        }

        public void DrawTooltip(bool? currentState, GetGroupDataDelegate data)
        {
            using var table = Im.Table.Begin("t"u8, 2, TableFlags.SizingFixedFit);
            if (!table)
                return;

            table.SetupColumn("##a"u8, TableColumnFlags.WidthFixed);
            table.SetupColumn("##b"u8, TableColumnFlags.WidthStretch);

            preset.DrawState(table, currentState);
            if (preset.State is ModState.Inherited)
                return;

            preset.DrawPriority(table);

            var groupList = new List<(string, bool)>();
            foreach (var (group, groupData) in preset.Settings)
            {
                // Skip unknown groups.
                if (data.Invoke(group, out var name, out var single) is not { } groupOptions)
                    continue;

                groupList.Clear();
                groupList.EnsureCapacity(groupOptions.Count);
                SettingPresetData.DrawGroup(table, name!, single, groupData, groupOptions, groupList);
            }
        }

        private static void DrawGroup(in Im.TableDisposable table, string groupName, bool single, in GroupSettingData groupData,
            IReadOnlyList<(ModObjectIdentifier, bool)> data, List<(string, bool)> groupList)
        {
            groupList.Clear();
            foreach (var (option, currentState) in data)
            {
                var state = groupData.GetValue(option);
                switch (state)
                {
                    case OptionState.Disabled: groupList.Add((option.Name!, false)); break;
                    case OptionState.Enabled:  groupList.Add((option.Name!, true)); break;
                    case OptionState.Toggle:   groupList.Add((option.Name!, !currentState)); break;
                }
            }

            if (groupList.Count is 0)
                return;

            table.NextColumn();
            table.DrawHorizontalSeparator();
            ImEx.TextFrameAligned(groupName);
            Im.Line.NoSpacing();
            Im.Dummy(2 * Im.Style.ItemSpacing.X);
            table.NextColumn();
            var firstItem = single ? groupList.FirstOrDefault(i => i.Item2, groupList[0]) : groupList[0];
            if (firstItem.Item2)
                Im.Render.Checkmark(Im.Window.DrawList, Im.Cursor.ScreenPosition.AddY(Im.Style.FramePadding.Y), LunaStyle.SuccessForeground,
                    Im.Style.TextHeight);
            else
                Im.Render.Cross(Im.Window.DrawList, Im.Cursor.ScreenPosition.AddY(Im.Style.FramePadding.Y), LunaStyle.ErrorForeground,
                    Im.Style.TextHeight);
            Im.Cursor.X += Im.Style.FrameHeightWithSpacing;
            ImEx.TextFrameAligned(firstItem.Item1);

            if (single && firstItem.Item2)
                return;

            foreach (var (option, value) in groupList.Skip(1))
            {
                table.NextColumn();
                table.NextColumn();
                if (value)
                    Im.Render.Checkmark(Im.Window.DrawList, Im.Cursor.ScreenPosition.AddY(Im.Style.FramePadding.Y),
                        LunaStyle.SuccessForeground,
                        Im.Style.TextHeight);
                else if (!single)
                    Im.Render.Cross(Im.Window.DrawList, Im.Cursor.ScreenPosition.AddY(Im.Style.FramePadding.Y), LunaStyle.ErrorForeground,
                        Im.Style.TextHeight);
                Im.Cursor.X += Im.Style.FrameHeightWithSpacing;
                ImEx.TextFrameAligned(option);
            }
        }

        private void DrawState(in Im.TableDisposable table, bool? currentState)
        {
            if (preset.State is ModState.Ignored)
                return;

            table.DrawFrameColumn("State"u8);
            table.NextColumn();
            switch (preset.State)
            {
                case ModState.Enabled:
                case ModState.Toggle when currentState is not true:

                    Im.Render.Checkmark(Im.Window.DrawList, Im.Cursor.ScreenPosition.AddY(Im.Style.FramePadding.Y),
                        LunaStyle.SuccessForeground,
                        Im.Style.TextHeight);
                    Im.Cursor.X += Im.Style.FrameHeightWithSpacing;
                    ImEx.TextFrameAligned("Enabled"u8);
                    break;
                case ModState.Toggle:
                case ModState.Disabled:
                    Im.Render.Cross(Im.Window.DrawList, Im.Cursor.ScreenPosition.AddY(Im.Style.FramePadding.Y), LunaStyle.ErrorForeground,
                        Im.Style.TextHeight);
                    Im.Cursor.X += Im.Style.FrameHeightWithSpacing;
                    ImEx.TextFrameAligned("Disabled"u8);
                    break;
                case ModState.Inherited:
                    Im.Render.Dot(Im.Window.DrawList, Im.Cursor.ScreenPosition.AddY(Im.Style.FramePadding.Y), LunaStyle.InfoForeground,
                        Im.Style.TextHeight);
                    Im.Cursor.X += Im.Style.FrameHeightWithSpacing;
                    ImEx.TextFrameAligned("Inherited"u8);
                    break;
            }
        }

        private void DrawPriority(in Im.TableDisposable table)
        {
            if (!preset._hasPriority)
                return;

            table.DrawFrameColumn("Priority"u8);
            table.DrawFrameColumn($"{preset.Priority!.Value}");
        }

        public static bool FromClipboard(out SettingPresetData data)
        {
            data = SettingPresetData.Create();
            try
            {
                var base64  = Im.Clipboard.Get();
                var version = CompressionFunctions.FromCompressedBase64(base64, out var json);
                if (version is not SettingPreset.CurrentVersion)
                    throw new Exception($"Invalid version {version} for setting preset data.");

                var reader = new Utf8JsonReader(json.Span, JsonFunctions.ReaderOptions);
                if (!reader.Read() || reader.TokenType is not JsonTokenType.StartObject)
                    throw new JsonException("Invalid JSON.");

                var depth = reader.CurrentDepth;
                while (reader.Read())
                {
                    if (depth == reader.CurrentDepth && reader.TokenType is JsonTokenType.EndObject)
                        break;

                    data.ParseJsonProperties(ref reader);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public void ToClipboard()
        {
            var array = new ArrayBufferWriter<byte>();
            using (var writer = new Utf8JsonWriter(array, JsonFunctions.UnformattedOptions))
            {
                writer.WriteStartObject();
                preset.WriteJsonProperties(writer);
                writer.WriteEndObject();
                writer.Flush();
            }

            var data = CompressionFunctions.ToCompressedBase64(array.WrittenSpan, SettingPreset.CurrentVersion);
            Im.Clipboard.Set(data);
        }
    }
}
