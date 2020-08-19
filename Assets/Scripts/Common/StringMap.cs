public class StringMap {
  public const string EMPTY = "";
  public const string NONE = "无";
  public const string ICON_FILE_NAME = "iconFileName";
  public const string DESCRIPTION = "description";
  public const string SELECT_TITLE = "选中称号";
  public const string UNSELECT_TITLE = "摘下称号";
  public const string SWITCH = "Switch";
  public const string VALUE = "Value";
  public const string NAME = "name";
  public const string PRICE = "price";
  public const string CLASS = "class";
  public const string SETTLEMENT = "settlement";
  public const string ID = "id";
  public const string HP_RECOVERY = "hpRecovery";
  public const string ENERGY_RECOVERY = "energyRecovery";
  public const string BUFFS = "buffs";
  public const string EXP = "exp";
  public const string FIXED_CONTENTS = "fixedContents";
  public const string RANDOM_CONTENTS = "randomContents";
  public const string PROB = "probability";
  public const string PROPS = "props";
  public const string NUM = "num";
  public const string RANGE = "range";
  public const string MIN = "min";
  public const string MAX = "max";
  public const string PUTIN_BAG = "放入背包";
  public const string PUTIN_WH = "放入仓库";
  public const string UP_STAR_PROP = "upstarprop";
  public const string UP_STAR_PROPS = "upstarProps";
  public const string TRAIT = "characteristic";
  public const string ILL2_FILE_NAMES = "illustration2FileNames";
  public const string BEEN_TOP_STAR = "已达到最高星级！";
  public const string PROP_NOT_ENOUGH = "材料不足！";
  public const string UP_STAR_SUCCESS = "升星成功！";
  public const string INPUT_EXCHANGE_CODE = "请输入兑换码！";
  public const string EXCHANGE_CODE_ERROR = "兑换码错误！";
  public const string EXCHANGE_CODE = "1053271651";
  public const string TOTAL_PRICE = "总价";
  public const string PROP_IN_SETTLEMENT = "propinsettlement";
  public const string TODAY = "今天";
  public const string SOME_DAY_BEFORE = "{0}天前";
  public const string SOME_WEEK_BEFORE = "{0}周前";
  public const string SOME_MONTH_BEFORE = "{0}月前";
  public const string SOME_YEAR_BEFORE = "{0}年前";
  public const string GAME_DATA_FILE = "GameData.bin";
  public const string MONSTERS = "monsters";
  public const string PERSONALITIES = "personalities";
  public const string TITLE = "title";
  public const string TITLES = "titles";
  public const string MAPS = "maps";
  public const string MAIN_STORY_LETTERS = "mainStoryLetters";
  public const string MAIN_STORY_MAILS = "mainStoryMails";
  public const string ACTIVITY_GIFTS = "activityGifts";
  public const string NPCS = "npcs";
  public const string TASK = "task";
  public const string TASK_ID = "taskId";
  public const string TASK_TYPE = "taskType";
  public const string TASKS = "tasks";
  public const string RARITY = "rarity";
  public const string TEXT = "text";
  public const string SIGNATURE = "signature";
  public const string GET_EXP = "获得经验值{0}";
  public const string LEVEL_UP_TO = "等级提升至Lv{0}";
  public const string GET_ACHIVEMENT = "获得成就";
  public const string REPEATED_PROP = "repeatedProp";
  public const string GET = "获得";
  public const string ME = "我";
  public const string CHAT = "chat";
  public const string CHAT_TRIGGER = "chatTrigger";
  public const string ACCEPT_TASK = "接受任务：[{0}]";
  public const string DIALOG = "dialog";
  public const string CATEGORY = "category";
  public const string ILL_INDEX = "illustrationIndex";
  public const string ILL_FILE_NAMES = "illustrationFileNames";
  public const string ROLE = "role";
  public const string ROLE_ID = "roleId";

  public static string CBTypeToString(CBType cbType) {
    switch (cbType) {
      case CBType.DROP: {
          return "丢弃";
        }
      case CBType.OK: {
          return "确认";
        }
      case CBType.SELL: {
          return "出售";
        }
      case CBType.USE: {
          return "使用";
        }
      default: {
          return "";
        }
    }
  }
}