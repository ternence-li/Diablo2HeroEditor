namespace Diablo2FileFormat.Interfaces
{
    public interface IQuestData
    {
        /// <summary>
        /// Changes the completion state of a specific quest.
        /// </summary>
        /// <param name="difficulty"></param>
        /// <param name="act"></param>
        /// <param name="quest"></param>
        /// <param name="complete">Quest completion state.</param>
        void ChangeQuest(Difficulty difficulty, Act act, Quest quest, bool complete);

        /// <summary>
        /// Changes all quest completion states of a specific act.
        /// </summary>
        /// <param name="difficulty"></param>
        /// <param name="act"></param>
        /// <param name="complete">Quest completion state.</param>
        void ChangeQuests(Difficulty difficulty, Act act, bool complete);

        /// <summary>
        /// Changes all quest completion states of a specific difficulty.
        /// </summary>
        /// <param name="difficulty"></param>
        /// <param name="complete"></param>
        void ChangeQuests(Difficulty difficulty, bool complete);

        /// <summary>
        /// Changes all quest completion states.
        /// </summary>
        /// <param name="complete">Quest completion state.</param>
        void ChangeQuests(bool complete);
    }
}
