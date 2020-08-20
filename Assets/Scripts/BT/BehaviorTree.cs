using System;
using System.Collections.Generic;

namespace MyBehaviorTree {
  //状态枚举类型
  public enum BTNodeResult {
    Failure,
    Success,
    Running,
  }
  public enum BTNodeType {
    Condition,
    Action,
    Selector,
    Sequence,
    Parallel,
    Decorator,
  }
  //一些初始化的静态方法
  public class BT {
    public static ConditionNode ConditionNode(Func<bool> conditionFunc) {
      return new ConditionNode(conditionFunc);
    }
    public static ActionNode ActionNode(Action immediateAction) {
      return new ActionNode(immediateAction);
    }
    public static ActionNode ActionNode(Func<IEnumerator<BTNodeResult>> continuousAction) {
      return new ActionNode(continuousAction);
    }
    public static SelectorNode SelectorNode() {
      return new SelectorNode();
    }
    public static SequenceNode SequenceNode() {
      return new SequenceNode();
    }
    public static ParallelNode ParallelNode() {
      return new ParallelNode();
    }
    public static InverseDecoratorNode InverseDecoratorNode(BaseNode childeNode) {
      return new InverseDecoratorNode(childeNode);
    }
  }
  /// <summary>
  /// 所有节点的基类
  /// </summary>
  public abstract class BaseNode {
    public BTNodeType nodeType;//节点类型
    public int nodeIndex; //在父节点的子节点列表里的索引号
    public BaseNode(BTNodeType nodeType) {
      this.nodeType = nodeType;
    }
    public abstract BTNodeResult Execute(); //执行方法
  }
  /// <summary>
  /// 组合节点类
  /// </summary>
  public abstract class CombinerNode : BaseNode {
    protected List<BaseNode> childNodeList = new List<BaseNode>();
    public CombinerNode(BTNodeType nodeType) : base(nodeType) { }
    public CombinerNode AddNode(params BaseNode[] nodes) {
      int startIndex = childNodeList.Count;
      for (int i = 0; i < nodes.Length; i++) {
        nodes[i].nodeIndex = startIndex + i;
        childNodeList.Add(nodes[i]);
      }
      return this;
    }
  }
  /// <summary>
  /// 条件节点
  /// </summary>
  public class ConditionNode : BaseNode {
    Func<bool> conditionFunc;
    public ConditionNode(Func<bool> conditionFunc) : base(BTNodeType.Condition) {
      this.conditionFunc = conditionFunc;
    }
    public override BTNodeResult Execute() {
      return conditionFunc() ? BTNodeResult.Success : BTNodeResult.Failure;
    }
  }
  /// <summary>
  /// 行为节点
  /// </summary>
  public class ActionNode : BaseNode {
    Action immediateAction; //瞬时行为
    Func<IEnumerator<BTNodeResult>> continuousAction;//持续行为
    IEnumerator<BTNodeResult> coroutine;

    public ActionNode(Action immediateAction) : base(BTNodeType.Action) {
      this.immediateAction = immediateAction;
    }
    public ActionNode(Func<IEnumerator<BTNodeResult>> continuousAction) : base(BTNodeType.Action) {
      this.continuousAction = continuousAction;
    }
    public override BTNodeResult Execute() {
      if (immediateAction != null) {
        immediateAction();
        return BTNodeResult.Success;
      }
      if (coroutine == null) {
        coroutine = continuousAction();
        return BTNodeResult.Running;
      }
      coroutine.MoveNext();
      BTNodeResult result = coroutine.Current;
      if (result != BTNodeResult.Running) {
        coroutine = null;
      }
      return result;
    }
  }
  /// <summary>
  /// 选择节点
  /// </summary>
  public class SelectorNode : CombinerNode {
    private BaseNode lastRunningNode;
    public SelectorNode() : base(BTNodeType.Selector) { }
    public override BTNodeResult Execute() {
      int index = 0;
      if (lastRunningNode != null) {
        index = lastRunningNode.nodeIndex;
      }
      lastRunningNode = null;
      BTNodeResult resultType = BTNodeResult.Failure;
      for (int i = index; i < childNodeList.Count; i++) {
        BaseNode tempNode = childNodeList[i];
        resultType = tempNode.Execute();
        if (resultType == BTNodeResult.Failure) {
          continue;
        }
        if (resultType == BTNodeResult.Success) {
          break;
        }
        if (resultType == BTNodeResult.Running) {
          lastRunningNode = tempNode;
          break;
        }
      }
      return resultType;
    }
  }
  /// <summary>
  /// 顺序节点
  /// </summary>
  public class SequenceNode : CombinerNode {
    private BaseNode lastRunningNode;
    public SequenceNode() : base(BTNodeType.Sequence) { }
    public override BTNodeResult Execute() {
      int index = 0;
      if (lastRunningNode != null) {
        index = lastRunningNode.nodeIndex;
      }
      lastRunningNode = null;
      BTNodeResult resultType = BTNodeResult.Failure;
      for (int i = index; i < childNodeList.Count; i++) {
        BaseNode tempNode = childNodeList[i];
        resultType = tempNode.Execute();

        if (resultType == BTNodeResult.Failure) {
          break;
        }
        if (resultType == BTNodeResult.Success) {
          continue;
        }
        if (resultType == BTNodeResult.Running) {
          lastRunningNode = tempNode;
          break;
        }
      }
      return resultType;
    }
  }
  /// <summary>
  /// 并行节点
  /// </summary>
  public class ParallelNode : CombinerNode {
    public ParallelNode() : base(BTNodeType.Parallel) { }
    public override BTNodeResult Execute() {
      int successCount = 0;
      int runningCount = 0;
      for (int i = 0; i < childNodeList.Count; i++) {
        BaseNode tempNode = childNodeList[i];
        BTNodeResult tempResult = tempNode.Execute();
        if (tempResult == BTNodeResult.Success) {
          successCount++;
        } else if (tempResult == BTNodeResult.Running) {
          runningCount++;
        }
      }
      if (runningCount > 0) {
        return BTNodeResult.Running;
      } else if (successCount > 0) {
        return BTNodeResult.Success;
      } else {
        return BTNodeResult.Failure;
      }
    }
  }
  /// <summary>
  /// 修饰节点(取反)
  /// </summary>
  public class InverseDecoratorNode : BaseNode {
    private BaseNode childNode;
    public InverseDecoratorNode(BaseNode childNode) : base(BTNodeType.Decorator) {
      this.childNode = childNode;
    }
    public override BTNodeResult Execute() {
      BTNodeResult tempResult = childNode.Execute();
      if (tempResult == BTNodeResult.Failure) {
        return BTNodeResult.Success;
      } else {
        return BTNodeResult.Failure;
      }
    }
  }
}