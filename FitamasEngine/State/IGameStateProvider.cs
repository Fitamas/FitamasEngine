using R3;

namespace Fitamas.State
{
    public interface IGameStateProvider
    {
        //public GameStateProxy GameState { get; }
        //public GameSettingsStateProxy SettingsState { get; }

        public Observable<bool> LoadGameState();
        public Observable<bool> LoadSettingsState();
        public Observable<bool> SaveGameState();
        public Observable<bool> SaveSettingsState();
        public Observable<bool> ResetGameState();
        public Observable<bool> ResetSettingsState();
    }
}
