using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheRuleTester : MonoBehaviour {

    public RuleData[] m_rules;
    public int m_iterations;
    public bool m_ignoreHigherPriority;

    [System.Serializable]
    public struct TestResult
    {
        public RuleData m_ExpectedTrigger;
        public RuleData m_ReceivedTrigger;
        public List<CustomerData> m_customers;
        public List<CustomerData.SpeedTier> m_tiers;
        public int m_incidents;

        public void AddCustomer(CustomerData data)
        {
            if(m_customers == null)
            {
                m_customers = new List<CustomerData>();
                m_tiers = new List<CustomerData.SpeedTier>();
            }
            m_customers.Add(data);
            m_tiers.Add(data.m_speedTier);
            m_incidents = m_customers.Count;
        }
    }

    public int m_totalErrors = 0;
    public List<TestResult> m_ErrorResults;

    public void RunTest()
    {
        m_totalErrors = 0;
        m_ErrorResults.Clear();
        RuleManager.GetInstance().ClearAllRules();
        foreach(RuleData rule in m_rules)
        {
            rule.Init();
            RuleManager.Instance.AddRule(rule);
        }

        for(int i=0; i<m_iterations; i++)
        {
            UnityEditor.EditorUtility.DisplayProgressBar("Running Rule Tester", string.Format("Running {0}/{1}, {2} errors", i, m_iterations, m_totalErrors), (float)i / (float)m_iterations);
            RunSingleTest();
        }
        UnityEditor.EditorUtility.ClearProgressBar();
    }

	private void RunSingleTest()
    {
        CustomerData data = ScriptableObject.CreateInstance<CustomerData>();
        data.Generate();
        RuleResponse triggeredRule = RuleManager.Instance.GetRuleForCustomer(data);

        if(triggeredRule.m_rule != data.m_failTrigger && (!m_ignoreHigherPriority || triggeredRule.m_rule.m_priority <= data.m_failTrigger.m_priority))
        {
            m_totalErrors++;
            bool foundMatch = false;
            for(int i=0; i< m_ErrorResults.Count; i++)
            {
                TestResult tr = m_ErrorResults[i];
                if(tr.m_ExpectedTrigger == data.m_failTrigger && tr.m_ReceivedTrigger == triggeredRule.m_rule)
                {
                    tr.AddCustomer(data);
                    foundMatch = true;
                    break;
                }
            }
            if(!foundMatch)
            {
                TestResult tr = new TestResult();
                tr.m_ExpectedTrigger = data.m_failTrigger;
                tr.m_ReceivedTrigger = triggeredRule.m_rule;
                tr.AddCustomer(data);
                m_ErrorResults.Add(tr);
            }
        }
    }
}
