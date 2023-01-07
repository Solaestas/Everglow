using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Sources.Modules.WorldModule
{
    public class ProgressToken
    {
        public ProgressToken(params Action<State>[] wheninvalids)
        {
            WhenInvalid += (state) =>
            {
                if (state == State.FailedByException)
                {
                    Everglow.Instance.Logger.Error(ToString());
                }
                else if (state == State.FailedByOther)
                {
                    Everglow.Instance.Logger.Debug(ToString());
                }
            };
        }
        bool iscancelled;
        List<string> logs = new();
        public event Action<State> WhenInvalid;
        public bool IsCancelled
        {
            get => iscancelled;
            set
            {
                iscancelled = iscancelled || value;
                if(!value)
                {
                    TokenState = State.FailedByUserCancellation;
                    logs.Add($"[{DateTime.Now:HH-mm-ss}]The user canceled the operation.");
                    WhenInvalid(TokenState);
                }
            }
        }
        public IReadOnlyList<string> Logs => logs;
        public State TokenState { get; private set; } = State.Waiting;
        public override string ToString()
        {
            return
                $"State:{TokenState}\n" +
                $"Logs:{string.Join("\n\t", Logs)}";
        }
        internal void StartRun()
        {
            TokenState = State.Running;
            logs.Add($"[{DateTime.Now:HH-mm-ss}]Start Running");
        }
        internal void Wait()
        {
            TokenState = State.Waiting;
            logs.Add($"[{DateTime.Now:HH-mm-ss}]Waiting");
        }
        internal void StopByOther(string reason)
        {
            iscancelled = true;
            TokenState = State.FailedByOther;
            logs.Add($"[{DateTime.Now:HH-mm-ss}]{reason}");
            WhenInvalid(TokenState);
        }
        internal void Over()
        {
            TokenState = State.ProgressOver;
            WhenInvalid(TokenState);
        }
        internal void Log(string log) => logs.Add($"[{DateTime.Now:HH-mm-ss}]{log}");
        internal void Exception(Exception e)
        {
            iscancelled = true;
            TokenState = State.FailedByException;
            logs.Add($"[{DateTime.Now:HH-mm-ss}]\n{e}");
            WhenInvalid(TokenState);
        }
        public enum State
        {
            Waiting,
            Running,
            FailedByException,
            FailedByUserCancellation,
            FailedByOther,
            ProgressOver
        }
    }
}