using System.ComponentModel;

namespace DocumentProcessor.Enums
{
    public enum Status
    {
        [Description("В ожидании")]
        NotStarted = 1,
        [Description("В работе")]
        InProgress,
        [Description("В оперативном архиве")]
        Finished,
        [Description("Отмена")]
        Rejected
    }
}
