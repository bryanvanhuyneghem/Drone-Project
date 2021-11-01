<?xml version="1.0" encoding="utf-8"?>
<TemplateLibrary xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <StaticCodeMode>false</StaticCodeMode>
  <Templates>
    <Template>
      <TemplateId>ba36fc72-3495-4388-9479-431def09ea24</TemplateId>
      <TemplateName>BasicSample</TemplateName>
      <Extends>ba36fc72-3495-4388-9479-431def09ea24</Extends>
      <CustomCode />
      <Fields>
        <Field>
          <Name>a</Name>
          <Code>5</Code>
          <UnitTest />
          <DataType>dynamic</DataType>
          <IncludeInOutput>true</IncludeInOutput>
          <SortOrder>1000</SortOrder>
        </Field>
        <Field>
          <Name>b</Name>
          <Code>a+5</Code>
          <UnitTest />
          <DataType>dynamic</DataType>
          <IncludeInOutput>true</IncludeInOutput>
          <SortOrder>1001</SortOrder>
        </Field>
        <Field>
          <Name>c</Name>
          <Code>"Hello World"</Code>
          <UnitTest />
          <DataType>dynamic</DataType>
          <IncludeInOutput>true</IncludeInOutput>
          <SortOrder>1002</SortOrder>
        </Field>
        <Field>
          <Name>d</Name>
          <Code>string s1 = "Hello";
string s2 = "World";

string s = s1 + " " + s2;

return s;</Code>
          <UnitTest />
          <DataType>dynamic</DataType>
          <IncludeInOutput>true</IncludeInOutput>
          <SortOrder>1003</SortOrder>
        </Field>
        <Field>
          <Name>e</Name>
          <Code>
//custom function. See Template Library settings/Code Modules

SampleModule.HelloWorld()</Code>
          <DataType>dynamic</DataType>
          <IncludeInOutput>true</IncludeInOutput>
          <SortOrder>1004</SortOrder>
        </Field>
      </Fields>
      <SortOrder>1</SortOrder>
    </Template>
    <Template>
      <TemplateId>b7faec15-c703-435a-a5a5-d3abde65bc12</TemplateId>
      <TemplateName>InheritanceSample</TemplateName>
      <Extends>b7faec15-c703-435a-a5a5-d3abde65bc12</Extends>
      <CustomCode />
      <Fields>
        <Field>
          <Name>a</Name>
          <Code>"A"</Code>
          <DataType>dynamic</DataType>
          <IncludeInOutput>true</IncludeInOutput>
          <SortOrder>1000</SortOrder>
        </Field>
        <Field>
          <Name>b</Name>
          <Code>"Hello " + a</Code>
          <DataType>dynamic</DataType>
          <IncludeInOutput>true</IncludeInOutput>
          <SortOrder>1001</SortOrder>
        </Field>
        <Field>
          <Name>c</Name>
          <Code>"C"</Code>
          <DataType>dynamic</DataType>
          <IncludeInOutput>true</IncludeInOutput>
          <SortOrder>1002</SortOrder>
        </Field>
      </Fields>
      <SortOrder>2</SortOrder>
    </Template>
    <Template>
      <TemplateId>9290af38-19ee-4a6f-ba3d-9f4d757e7483</TemplateId>
      <TemplateName>ChildTemplate</TemplateName>
      <Extends>b7faec15-c703-435a-a5a5-d3abde65bc12</Extends>
      <CustomCode />
      <Fields>
        <Field>
          <Name>a</Name>
          <Code>"this value overrides base value of A"</Code>
          <DataType>dynamic</DataType>
          <IncludeInOutput>true</IncludeInOutput>
          <Description>overrides</Description>
          <SortOrder>1000</SortOrder>
        </Field>
        <Field>
          <Name>d</Name>
          <Code>"this value does not exist in base template"</Code>
          <DataType>dynamic</DataType>
          <IncludeInOutput>true</IncludeInOutput>
          <Description>extends</Description>
          <SortOrder>1001</SortOrder>
        </Field>
      </Fields>
      <SortOrder>1</SortOrder>
    </Template>
    <Template>
      <TemplateId>e3c15edd-c187-4595-a277-8ffa81142c35</TemplateId>
      <TemplateName>Disney</TemplateName>
      <Extends>e3c15edd-c187-4595-a277-8ffa81142c35</Extends>
      <CustomCode />
      <Fields>
        <Field>
          <Name>executive_officers_age</Name>
          <Code>p.Reset()
.Find("Executive Officers of the Company")
.Find("Name")
.Table(new PdfTableOptions(){ WhiteSpaceLimit = 1.5f})
.AsEnumerable()
.Average(x=&gt;int.Parse(x["Age"].ToString()))
</Code>
          <DataType>dynamic</DataType>
          <IncludeInOutput>true</IncludeInOutput>
          <Description>Table + Linq</Description>
          <SortOrder>1001</SortOrder>
        </Field>
        <Field>
          <Name>financial_data</Name>
          <Code>p.Reset()
.Filter(t=&gt;t.Height &gt; 7)
.Filter(t=&gt;t.Text != "$")
.Find("(in millions, except per share data)")
.Below()
.Table(new PdfTableOptions(){ WhiteSpaceLimit = 1.5f})
.DeleteRows(x=&gt;x[1].ToString() == "")
.ToNumber(1,2,3,4,5)
.NameColumns("Category")</Code>
          <UnitTest>if (value.HasEmptyColumns()) return false;

if (value.Rows.Count == 0) return false;

return true;</UnitTest>
          <DataType>DataTable</DataType>
          <IncludeInOutput>true</IncludeInOutput>
          <Description>Table example. Custom validation</Description>
          <SortOrder>1002</SortOrder>
        </Field>
        <Field>
          <Name>financial_analysis</Name>
          <Code>p.Reset()
.Find("ITEM 7. Management’s Discussion and Analysis of Financial Condition and Results of Operations")
.Find(t=&gt;t.Bold)
.Filter(t=&gt;t.Text != "$" &amp;&amp; t.Text != "%")
.Table(new PdfTableOptions(){ WhiteSpaceLimit = 1.5f,
HeaderSeparatedByLine = true})
.DeleteRows(r=&gt;r[0].ToString().Contains(":"))
.Rollup(1)
.ToNumber(1,2,3)
.NameColumns("Category")</Code>
          <UnitTest>//we can set specific error message by throwing exception
if (value.Rows.Count &lt; 30)
{
    throw new Exception ("Expecting more records");
}

return true;</UnitTest>
          <DataType>dynamic</DataType>
          <IncludeInOutput>true</IncludeInOutput>
          <Description>Another table</Description>
          <SortOrder>1003</SortOrder>
        </Field>
        <Field>
          <Name>espn_subscribers</Name>
          <Code>p.Reset()
.Find("ESPN - Domestic")
.Find("ESPN")
.Right()
.Text</Code>
          <DataType>dynamic</DataType>
          <IncludeInOutput>true</IncludeInOutput>
          <Description>Single value</Description>
          <SortOrder>1000</SortOrder>
        </Field>
        <Field>
          <Name>directors_list</Name>
          <Code>//use linq to list everyone on the board of directors

p.Reset()
.Find("BOARD OF DIRECTORS")
.Down()
.FilterOffset(0,0,120, 700) //filter for the left column
.Filter(x=&gt;x.Bold) //filter for tokens in BOLD
.Tokens.Select(x=&gt;x.Text).ToList(); //get token texts</Code>
          <UnitTest>return (value.Count &gt;0)</UnitTest>
          <DataType>dynamic</DataType>
          <IncludeInOutput>true</IncludeInOutput>
          <Description>Filter + Linq</Description>
          <SortOrder>1004</SortOrder>
        </Field>
        <Field>
          <Name>email_list</Name>
          <Code>//Find all emails in the document

DataTable dt = new DataTable();
dt.AddColumn("Email", null, typeof(string));

p.Reset();

//search through the document until @ can be found
while (true)
{
    try
    {
        p.Find("@");
        
        DataRow dr = dt.NewRow();
        dr["Email"] = p.Text;
        
        dt.Rows.Add(dr);
    }
    catch
    {
        break;
    }
}

return dt;</Code>
          <DataType>dynamic</DataType>
          <IncludeInOutput>true</IncludeInOutput>
          <Description>loop example</Description>
          <SortOrder>1005</SortOrder>
        </Field>
      </Fields>
      <Description>test with disney.ipb</Description>
      <SortOrder>3</SortOrder>
    </Template>
  </Templates>
  <ReferenceList>System.dll
System.Data.dll
System.Data.DataSetExtensions.dll
System.Xml.dll
System.Core.dll
System.Linq.dll
Microsoft.CSharp.dll
IvyPdf.dll</ReferenceList>
  <UsingList>System
System.Data
System.Text.RegularExpressions
System.Linq
IvyPdf</UsingList>
  <MaxExecutionTime>0</MaxExecutionTime>
  <CodeModules>
    <CodeModule>
      <ModuleName>SampleModule</ModuleName>
      <ModuleCode>public static class SampleModule
{
    public static string HelloWorld()
    {
        return "Hello World";
    }


    //Extension methods can be used to extend functionality of existing types
    public static bool HasEmptyColumns(this DataTable dt)
    {
        for(int j = 0; j&lt; dt.Columns.Count; j++)
        {
            bool empty = true;
            for (int i = 0; i &lt; dt.Rows.Count; i++)
            {
                if (dt.Rows[i][j].ToString().Trim() != "") 
                {
                    empty = false;
                    break;
                }
            }
            if (empty) return true;
        }
        return false;
    }

}</ModuleCode>
    </CodeModule>
  </CodeModules>
</TemplateLibrary>