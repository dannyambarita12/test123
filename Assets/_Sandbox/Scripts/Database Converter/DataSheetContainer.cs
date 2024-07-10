using Cathei.BakingSheet;

public class DataSheetContainer : SheetContainerBase
{
    public DataSheetContainer(Microsoft.Extensions.Logging.ILogger logger) : base(logger)
    {
    }

    public CardSheet AraCards { get; set; }
    public CardSheet GluttonCards { get; set; }
    public CardSheet AiraCards { get; set; }
}

public class CardSheet : Sheet<CardSheet.Row>
{
    public class Row : SheetRowArray<Elem>
    {
        public string Name { get; private set; }
        public CardDistanceType Distance { get; private set; }
        public CardActionType Action { get; private set; }
        public BaseStatType StatType { get; private set; }
        public int EP { get; private set; }

        public int TokenCount => Count;

        public Elem GetToken(int index)
        {
            return this[index];
        }
    }

    public class Elem : SheetRowElem
    {
        public CardTokenType TokenType { get; private set; }
        public int Min { get; private set; }
        public int Max { get; private set; }
    }
}