namespace Model.ViewModel
{
    public class CsPartsView
    {
        public class CsPartsWhere
        {
            public string PartName { get; set; } = string.Empty;
            public int PartType { get; set; } = 0;
            public int PartState { get; set; } = -1;
        }

        public class CsPartList
        {
            public int PartId { get; set; } = 0;
            public string PartType { get; set; } = string.Empty;
            public string PartName { get; set; } = string.Empty;
            public string PartWeight { get; set; } = string.Empty;
            public string PartPrice { get; set; } = string.Empty;
            public string OperationDate { get; set; } = string.Empty;
            public string PartState { get; set; } = string.Empty;
        }
    }
}
