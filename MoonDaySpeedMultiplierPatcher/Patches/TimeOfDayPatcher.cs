using HarmonyLib;
using MoonDaySpeedMultiplierPatcher.Util;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace MoonDaySpeedMultiplierPatcher.Patches
{
    [HarmonyPatch(typeof(TimeOfDay))]
    internal static class TimeOfDayPatcher
    {
        [HarmonyPatch(nameof(TimeOfDay.MoveGlobalTime))]
        [HarmonyTranspiler]
        static IEnumerable<CodeInstruction> MoveGlobalTimeTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            FieldInfo currentLevel = typeof(TimeOfDay).GetField(nameof(TimeOfDay.currentLevel));
            FieldInfo DaySpeedMultiplier = typeof(SelectableLevel).GetField(nameof(SelectableLevel.DaySpeedMultiplier));
            FieldInfo globalTimeSpeedMultiplier = typeof(TimeOfDay).GetField(nameof(TimeOfDay.globalTimeSpeedMultiplier));

            List<CodeInstruction> codes = new(instructions);
            int index = 0;
            Tools.FindField(ref index, ref codes, findField: globalTimeSpeedMultiplier, skip: true, errorMessage: "Couldn't find the global time speed multiplier field");
            codes.Insert(index, new CodeInstruction(opcode: OpCodes.Mul));
            codes.Insert(index, new CodeInstruction(opcode: OpCodes.Ldfld, operand: DaySpeedMultiplier));
            codes.Insert(index, new CodeInstruction(opcode: OpCodes.Ldfld, operand: currentLevel));
            codes.Insert(index, new CodeInstruction(opcode: OpCodes.Ldarg_0));
            return codes;
        }
        [HarmonyPatch(nameof(TimeOfDay.Update))]
        [HarmonyTranspiler]
        static IEnumerable<CodeInstruction> UpdateTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            FieldInfo DaySpeedMultiplier = typeof(SelectableLevel).GetField(nameof(SelectableLevel.DaySpeedMultiplier));

            int index = 0;
            List<CodeInstruction> codes = new(instructions);
            Tools.FindField(ref index, ref codes, findField: DaySpeedMultiplier, skip: true, errorMessage: "Couldn't find the day speed multiplier field");
            codes.RemoveRange(index-3, 3);
            codes.Insert(index - 3, new CodeInstruction(opcode: OpCodes.Ldc_R4, operand: 1f));
            return codes;
        }
        [HarmonyPatch(nameof(TimeOfDay.CalculatePlanetTime))]
        [HarmonyTranspiler]
        static IEnumerable<CodeInstruction> CalculatePlanetTimeTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            FieldInfo DaySpeedMultiplier = typeof(SelectableLevel).GetField(nameof(SelectableLevel.DaySpeedMultiplier));

            int index = 0;
            List<CodeInstruction> codes = new(instructions);
            Tools.FindField(ref index, ref codes, findField: DaySpeedMultiplier, skip: true, errorMessage: "Couldn't find the day speed multiplier field");
            codes.RemoveRange(index - 2, 2);
            codes.Insert(index - 2, new CodeInstruction(opcode: OpCodes.Ldc_R4, operand: 1f));
            return codes;
        }
    }
}
