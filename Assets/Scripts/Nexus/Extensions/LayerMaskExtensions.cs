using System;
using UnityEngine;

namespace Assets.Scripts.ClientUtilities.Extensions
{
 public static class LayerMaskHelper
 {
  public static bool IsContains(this LayerMask Orginal, int LayerValue)
  {
   return ((Orginal & (1 << LayerValue)) != 0);
  }

  public static bool IsContains(this LayerMask Original, LayerMask LayerValue)
  {
   return ((Original.value & (1 << LayerValue.value)) != 0);
  }

 }
}