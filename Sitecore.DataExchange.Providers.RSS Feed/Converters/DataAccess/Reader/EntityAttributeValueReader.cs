using System;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using Sitecore.DataExchange.DataAccess;
using Sitecore.DataExchange.DataAccess.Readers;


namespace Sitecore.DataExchange.Providers.RssFeed.Converters.DataAccess.Reader
{

    namespace Sitecore.DataExchange.Providers.DynamicsCrm.DataAccess.Readers
    {
       public class RssFeedFieldValueReader : IValueReader
        {
       
            public readonly string FieldName;

            public bool UseValueProperty { get; set; }

            public RssFeedFieldValueReader(string fieldName)
            {
                this.FieldName = fieldName;

            }

            public CanReadResult CanRead(object source, DataAccessContext context)
            {
                bool flag = source != null && source is SyndicationItem;
                return new CanReadResult()
                {
                    CanReadValue = flag
                };
            }

            public ReadResult Read(object source, DataAccessContext context)
            {
                var flag = false;
                object readValue = (object)null;
                var feeditem = source as SyndicationItem;
                if (feeditem != null)
                {
                    if (FieldName == "Id" && !string.IsNullOrEmpty(feeditem.Id))
                    {
                        readValue = feeditem.Id;
                        flag = true;
                    }
                    else if (FieldName == "Title" && !string.IsNullOrEmpty(feeditem.Title.Text))
                    {
                        readValue = feeditem.Title.Text;
                        flag = true;
                    }
                    // ReSharper disable once ConditionIsAlwaysTrueOrFalse
                    else if (FieldName == "Links" && (feeditem.Links != null))
                    {
                        var firstOrDefault = feeditem.Links.FirstOrDefault();
                        if (firstOrDefault != null)
                        {
                            readValue = firstOrDefault.Uri.ToString();
                            flag = true;
                        }
                    }
                    else if (FieldName == "Authors" && (feeditem.Authors.Count>0))
                    {
                        StringBuilder authors = new StringBuilder();
                        foreach(string name in feeditem.Authors.Select(person => person.Name))
                        {
                            authors.AppendFormat("{0}|", name);
                            flag = true;
                        }
                        readValue = authors.ToString();
                    }
                    else if (FieldName == "Content" && (feeditem.Content != null))
                    {
                        readValue =  feeditem.Content.ToString() ?? string.Empty;
                        flag = true;
                    }
                    else if (FieldName == "Summary" && (feeditem.Summary != null))
                    {
                        readValue = feeditem.Summary.Text;
                        flag = true;
                    }
                    else if (FieldName == "Copyright" && (feeditem.Copyright != null))
                    {
                        readValue = feeditem.Copyright.Text;
                        flag = true;
                    }
                    else if (FieldName == "Categories" && (feeditem.Categories.Count > 0))
                    {
                        StringBuilder catagories = new StringBuilder();
                        foreach (string catagory in feeditem.Categories.Select(catagoty => catagoty.Name))
                        {
                            catagories.AppendFormat("{0}|", catagory);
                            flag = true;
                        }
                        readValue = catagories.ToString();
                    }
                    else if (FieldName == "PublishDate")
                    {
                        readValue = feeditem.PublishDate.UtcDateTime.ToString("yyyyMMddTHHmmssZ");
                        flag = true; 
                    }
                    else if (FieldName == "Contributions" && (feeditem.Contributors.Count > 0))
                    { 
                        StringBuilder contributors = new StringBuilder();
                        foreach (string contributor in feeditem.Contributors.Select(contributor => contributor.Name))
                        {
                            contributors.AppendFormat("{0}|", contributor);
                            flag = true;
                        }
                        readValue = contributors.ToString();
                    }
                }
                return new ReadResult(DateTime.Now)
                {
                    WasValueRead = flag,
                    ReadValue = readValue
                };
            }
     
        }
    }
}
