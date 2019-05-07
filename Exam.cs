using System;
using System.Collections.Generic;
/// <summary>
/// Summary description for Class1
/// </summary>

//base class
public interface IFile
{
  
}

private enum AccountType
{
    Trading = 1,
    RRSP,
    RESP,
    Fund
}

//subClass A that implements the base class
public class AFile : IFile
{
    public AFile()
    {
        this.Accounts = new ICollection<AFileAccount>();
        this.HasHeader = true;
    }

    public string FileName;   
    public string FileType;
    public bool HasHeader;
    
    public ICollection<AFileAccount> Accounts { get; set; }
}
//subClass b that implements the base class
public class BFile :IFile
{
    public BFile()
    {
        this.Accounts = new ICollection<BFileAccount>();
        this.HasHeader = false;
    }
    public string FileName;
    public string FileType;
    public bool HasHeader;

    public ICollection<BFileAccount> Accounts { get; set; }
}
private class AFileAccount
{
    private string Identifier { get; set; }
    public string Name { get; set; }
    public int Type { get; set; }
    public bool Opened { get; set; }
    public string Currency { get; set; }
}

private class BFileAccount
{
    public string Name { get; set; }
    public int Type { get; set; }
    public string Currency { get; set; }
    public string CustodianCode { get; set; }
}
private class OutPutAccount
{   
    public string AccountCode { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public DateTime OpenDate { get; set; }
    public string Currency { get; set; }
}

//main class
public class Transformation
{
    public List<OutPutAccount> Transform(IFile file)
    {
        List account = new List<OutPutAccount>();
        if (file.GetType() == TypeOf(AFile))
        {
            //skip the header row, asume that the input file still contain the header
            foreach (AFileAccount a in ((AFile)file).Accounts.Skip(1))
            {
                account.Add(new OutPutAccount
                {
                    AccountCode = a.Identifier.Split('|')[1],
                    Name = a.Name,
                    Type = Enum.GetName(typeof(AccountType), a.Type),
                    Currency = a.Currency == "CD" ? "CAD" : "USD",
                    OpenDate = a.Opened
                });
            }

        }
        else if (file.GetType() == TypeOf(BFile))
        {
            foreach (BFileAccount b in ((BFile)file).Accounts)
            {
                account.Add(new OutPutAccount
                {
                    AccountCode = b.CustodianCode,
                    Name = b.Name,
                    Type = b.Type,
                    Currency = b.Currency == "C" ? "CAD" : "USD",
                   
                });
            }
        }
        return account;
    }
}
