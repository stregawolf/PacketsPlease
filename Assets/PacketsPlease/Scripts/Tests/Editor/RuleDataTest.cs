using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Text;

[TestFixture]
public class RuleDataTest {

    // [Test]
    // public void NewEditModeTestSimplePasses() {
    // 	// Use the Assert class to test conditions.
    // }

    // // A UnityTest behaves like a coroutine in PlayMode
    // // and allows you to yield null to skip a frame in EditMode
    // [UnityTest]
    // public IEnumerator NewEditModeTestWithEnumeratorPasses() {
    // 	// Use the Assert class to test conditions.
    // 	// yield to skip a frame
    // 	yield return null;
    // }

    RuleManager ruleManager;

    const int RANDOM_TEST_ITERATIONS = 100;
    
    string CUSTOMER_NAME = "John Doe";
    string ACTIVITY_NAME = "War of Worldcraft";
    ActivityData.Activity.Type ACTIVITY_TYPE = ActivityData.Activity.Type.GAME;
    float DATA_USAGE = 50f;
    int DAYS_ACTIVE = 100;
    bool IS_MALE = true;
    
    // Test activity types

    // Test Customers
    CustomerData customer;
    
    // Test actions
    ActionData boostCustomer;
    ActionData throttleCustomer;
    ActionData disconnectCustomer;

    [SetUp]
    public void Setup() {
        //GameObject ruleManagerObj = new GameObject();
        //ruleManagerObj.AddComponent<RuleManager>();

        ruleManager = RuleManager.Instance;

        Debug.Assert(ruleManager != null);

        customer = ScriptableObject.CreateInstance<CustomerData>();
        customer.m_name.Set(CUSTOMER_NAME);
        customer.m_dataUsage = DATA_USAGE;
        customer.m_daysActive = DAYS_ACTIVE;
        customer.m_activity = new ActivityData.Activity(ACTIVITY_NAME, ACTIVITY_TYPE);
        customer.m_male = IS_MALE;
            
        boostCustomer = new ActionData(customer, ActionType.Boost);
        throttleCustomer = new ActionData(customer, ActionType.Throttle);
        disconnectCustomer = new ActionData(customer, ActionType.Disconnect);
    }

    [Test]
    public void AddRulesAndClearThem() {
        RuleData testRule = new RuleData(ActionType.Throttle, RuleData.HIGHEST_PRIORITY);

        Debug.Assert(ruleManager.Rules.Count == 0, "ClearAtStart");
        ruleManager.AddRule(testRule);
        ruleManager.AddRule(testRule);
        ruleManager.AddRule(testRule);
        ruleManager.AddRule(testRule);
        ruleManager.AddRule(testRule);

        ruleManager.ClearAllRules();
        Debug.Assert(ruleManager.Rules.Count == 0, "ClearAtEnd");
    }

    private void AssertValidActionIs(ActionType type, CustomerData customer)
    {
        ActionData boost = new ActionData(customer, ActionType.Boost);
        ActionData throttle = new ActionData(customer, ActionType.Throttle);
        ActionData disconnect = new ActionData(customer, ActionType.Disconnect);

        Debug.Assert(type != ActionType.Boost || !ruleManager.DoesViolateRules(boost), DumpActionInfo(customer, boost));
        Debug.Assert(type != ActionType.Throttle || !ruleManager.DoesViolateRules(throttle), DumpActionInfo(customer, throttle));
        Debug.Assert(type != ActionType.Disconnect || !ruleManager.DoesViolateRules(disconnect), DumpActionInfo(customer, disconnect));
    }

    private string DumpActionInfo(CustomerData customer, ActionData actionData)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(actionData.actionType)
            .Append("\n")
            .Append(customer.ToString())
            .Append("\n")
            .Append(actionData.AllRulesToStr());

        return sb.ToString();
    }

    private CustomerData GetRandomCustomer()
    {
        CustomerData c = ScriptableObject.CreateInstance<CustomerData>();
        c.GenerateTrueRandom();
        return c;
    }

    [Test]
    public void IfNoRulesApplyDefault() {
        ruleManager.ClearAllRules();

        // Default action should pass
        AssertValidActionIs(RuleManager.DEFAULT_ACTION, customer);

        for(int i = 0; i < RANDOM_TEST_ITERATIONS; i++)
        {
            CustomerData c = GetRandomCustomer();
            AssertValidActionIs(RuleManager.DEFAULT_ACTION, c);
        }
    }

    [Test]
    public void BlanketRuleAppliesToEveryone() {

        RuleData throttleEveryone = new RuleData(ActionType.Throttle, RuleData.HIGHEST_PRIORITY);
        ruleManager.ClearAllRules();
        ruleManager.AddRule(throttleEveryone);

        AssertValidActionIs(ActionType.Throttle, customer);

        for(int i = 0; i < RANDOM_TEST_ITERATIONS; i++)
        {
            CustomerData c = GetRandomCustomer();
            AssertValidActionIs(ActionType.Throttle, c);
        }
    }

    [Test]
    public void ThrottleAbove50() {
        
        RuleData throttleAbove50 = new UsageHigherRule(50f, ActionType.Throttle, RuleData.HIGHEST_PRIORITY);
        ruleManager.ClearAllRules();
        ruleManager.AddRule(throttleAbove50);

        for(int i = 0; i < RANDOM_TEST_ITERATIONS; i++)
        {
            CustomerData c = GetRandomCustomer();

            if(c.m_dataUsage >= 50f)
            {
                AssertValidActionIs(ActionType.Throttle, c);
            }
            else
            {
                AssertValidActionIs(RuleManager.DEFAULT_ACTION, c);
            }
        }
    }

    [Test]
    public void ThrottleAboveBoostBelow50() {

        RuleData throttleAbove50 = new UsageHigherRule(50f, ActionType.Throttle, RuleData.HIGHEST_PRIORITY);
        RuleData boostBelow50 = new UsageLowerRule(50f, ActionType.Boost, RuleData.HIGHEST_PRIORITY);
        ruleManager.ClearAllRules();
        ruleManager.AddRule(throttleAbove50);
        ruleManager.AddRule(boostBelow50);

        CustomerData c = GetRandomCustomer();
        c.m_dataUsage = 49f;
        AssertValidActionIs(ActionType.Boost, c);
        c.m_dataUsage = 51f;
        AssertValidActionIs(ActionType.Throttle, c);
        c.m_dataUsage = 50f;
        AssertValidActionIs(ActionType.Throttle, c);

        for(int i = 0; i < RANDOM_TEST_ITERATIONS; i++)
        {
            CustomerData cc = GetRandomCustomer();

            if(cc.m_dataUsage >= 50f)
            {
                AssertValidActionIs(ActionType.Throttle, cc);
            }
            else
            {
                AssertValidActionIs(ActionType.Boost, cc);
            }
        }
    }

    [Test]
    public void CustomerMustSatisfyAllConstraintsInRule() {

    }

    [Test]    
    public void ConflictingRulesCheckPriority() {

    }
}
