﻿using System;

namespace TelegramBotBase.Exceptions
{
    public class MaximumRowsReachedException : Exception
    {
        public int Value { get; set; }

        public int Maximum { get; set; }


        public override string Message =>
            $"You have exceeded the maximum of rows by {Value.ToString()} / {Maximum.ToString()}";
    }
}