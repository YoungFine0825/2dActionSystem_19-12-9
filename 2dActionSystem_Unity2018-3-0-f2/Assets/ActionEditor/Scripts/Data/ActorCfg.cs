using LitJson;
namespace ActionGame.ActionSystem{


    public class ActorCfg
    {
        public int id = 0;
        public ActionInfo[] actionsInfo;
    }

    /*
     * 动作信息
     */
    public struct ActionInfo {
        public int id;
        public int keyframesCount;
        public string tag;
        public string name;
        public string animResUrl;
        public ActionKeyFrameInfo[] framesInfo;
    }

    /*
     * 动作帧信息
     */
    public struct ActionKeyFrameInfo
    {
        public int frameIndex;
        public int[] positionOffset;
        public HitBoxInfo[] hitBoxsInfo;
    }


    public struct HitBoxInfo
    {
        public int type;
        public string tag;
        public float x, y;//position
        public float width, height;//size
    }
}


