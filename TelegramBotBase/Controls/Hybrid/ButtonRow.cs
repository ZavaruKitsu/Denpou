﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TelegramBotBase.Form;

namespace TelegramBotBase.Controls.Hybrid
{
    [DebuggerDisplay("{Count} columns")]
    public class ButtonRow
    {
        private List<ButtonBase> __buttons = new List<ButtonBase>();

        public ButtonRow()
        {
        }

        public ButtonRow(params ButtonBase[] buttons)
        {
            __buttons = buttons.ToList();
        }


        public ButtonBase this[int index] => __buttons[index];

        public int Count => __buttons.Count;

        public void Add(ButtonBase button)
        {
            __buttons.Add(button);
        }

        public void AddRange(ButtonBase button)
        {
            __buttons.Add(button);
        }

        public void Insert(int index, ButtonBase button)
        {
            __buttons.Insert(index, button);
        }

        public IEnumerator<ButtonBase> GetEnumerator()
        {
            return __buttons.GetEnumerator();
        }

        public ButtonBase[] ToArray()
        {
            return __buttons.ToArray();
        }

        public List<ButtonBase> ToList()
        {
            return __buttons.ToList();
        }

        public bool Matches(string text, bool useText = true)
        {
            foreach (var b in __buttons)
            {
                if (useText && b.Text.Trim().Equals(text, StringComparison.InvariantCultureIgnoreCase))
                    return true;

                if (!useText && b.Value.Equals(text, StringComparison.InvariantCultureIgnoreCase))
                    return true;
            }

            return false;
        }

        /// <summary>
        ///     Returns the button inside of the row which matches.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="useText"></param>
        /// <returns></returns>
        public ButtonBase GetButtonMatch(string text, bool useText = true)
        {
            foreach (var b in __buttons)
            {
                if (useText && b.Text.Trim().Equals(text, StringComparison.InvariantCultureIgnoreCase))
                    return b;
                if (!useText && b.Value.Equals(text, StringComparison.InvariantCultureIgnoreCase))
                    return b;
            }

            return null;
        }

        public static implicit operator ButtonRow(List<ButtonBase> list)
        {
            return new ButtonRow { __buttons = list };
        }
    }
}