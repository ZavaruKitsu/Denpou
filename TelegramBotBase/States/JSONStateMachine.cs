﻿using System;
using System.IO;
using Newtonsoft.Json;
using TelegramBotBase.Args;
using TelegramBotBase.Base;
using TelegramBotBase.Form;
using TelegramBotBase.Interfaces;

namespace TelegramBotBase.States
{
    /// <summary>
    ///     Is used for all complex data types. Use if other default machines are not working.
    /// </summary>
    public class JSONStateMachine : IStateMachine
    {
        /// <summary>
        ///     Will initialize the state machine.
        /// </summary>
        /// <param name="file">Path of the file and name where to save the session details.</param>
        /// <param name="fallbackStateForm">
        ///     Type of Form which will be saved instead of Form which has
        ///     <seealso cref="Attributes.IgnoreState" /> attribute declared. Needs to be subclass of
        ///     <seealso cref="Form.FormBase" />.
        /// </param>
        /// <param name="overwrite">Declares of the file could be overwritten.</param>
        public JSONStateMachine(string file, Type fallbackStateForm = null, bool overwrite = true)
        {
            if (file is null) throw new ArgumentNullException(nameof(file));

            FallbackStateForm = fallbackStateForm;

            if (FallbackStateForm != null && !FallbackStateForm.IsSubclassOf(typeof(FormBase)))
                throw new ArgumentException("FallbackStateForm is not a subclass of FormBase");

            FilePath = file;
            Overwrite = overwrite;
        }

        public string FilePath { get; set; }

        public bool Overwrite { get; set; }

        public Type FallbackStateForm { get; }

        public StateContainer LoadFormStates()
        {
            try
            {
                var content = File.ReadAllText(FilePath);

                var sc = JsonConvert.DeserializeObject<StateContainer>(content, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All,
                    TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple
                });

                return sc;
            }
            catch
            {
            }

            return new StateContainer();
        }

        public void SaveFormStates(SaveStatesEventArgs e)
        {
            if (File.Exists(FilePath))
            {
                if (!Overwrite) throw new Exception("File exists already.");

                File.Delete(FilePath);
            }

            try
            {
                var content = JsonConvert.SerializeObject(e.States, Formatting.Indented, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All,
                    TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple
                });

                File.WriteAllText(FilePath, content);
            }
            catch
            {
            }
        }
    }
}