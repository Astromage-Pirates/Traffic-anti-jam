using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace AstroPirate.Ultilities
{
	//This class is non Serializable
	public class AutoDisposeList<T> : Collection<T> where T : IDisposable
	{
		protected override void RemoveItem(int index)
		{
			if (index >= 0 && index <= Count )
			{
				var item = this[index];
				item.Dispose();
			}
			base.RemoveItem(index);
		}

		protected override void ClearItems()
		{
            foreach (var item in this)
            {
				item.Dispose();
            }
            base.ClearItems();
		}

	}

}

