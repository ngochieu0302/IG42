using System.Collections.Generic;
using FDI.Simple;
using FDI.Utils;
namespace FDI.GetAPI
{
    public class LevelVoteAPI : BaseAPI
    {
        private readonly string _url = Domain;

	    public ModelLevelVoteItem GetListSimple(int? agencyId)
	    {
			var urlJson = string.Format("{0}LevelVote/GetListSimple?key={1}&agencyId={2}", Domain, Keyapi, agencyId);
		    return GetObjJson<ModelLevelVoteItem>(urlJson);
		}
        public List<LevelVoteItem> GetList(int agencyid)
	    {
		    var urlJson = string.Format("{0}LevelVote/GetList?key={1}&agencyId={2}", _url, Keyapi,agencyid);
		    return GetObjJson<List<LevelVoteItem>>(urlJson);
	    }
		public ModelLevelVoteItem ListItems(string url)
	    {
		    url = string.IsNullOrEmpty(url) ? "?" : url;
			var urlJson = string.Format("{0}LevelVote/ListItems{1}&key={2}", Domain, url, Keyapi);
		    return GetObjJson<ModelLevelVoteItem>(urlJson);
		}

	    public LevelVoteItem GetLevelVoteItem(int id)
	    {
		    var urlJson = string.Format("{0}LevelVote/GetLevelVoteItem?key={1}&id={2}", Domain, Keyapi, id);
		    return GetObjJson<LevelVoteItem>(urlJson);
	    }

		public JsonMessage Add(string json)
	    {
		    var urlJson = string.Format("{0}LevelVote/Add?key={1}&{2}", Domain, Keyapi, json);
		    return GetObjJson<JsonMessage>(urlJson);
	    }

	    public JsonMessage Update(string json)
	    {
		    var urlJson = string.Format("{0}LevelVote/Update?key={1}&{2}", Domain, Keyapi, json);
		    return GetObjJson<JsonMessage>(urlJson);
	    }
	    public JsonMessage Delete(string lstArrId)
	    {
		    var urlJson = string.Format("{0}LevelVote/Delete?key={1}&lstArrId={2}", Domain, Keyapi, lstArrId);
		    return GetObjJson<JsonMessage>(urlJson);
	    }

	}
}
