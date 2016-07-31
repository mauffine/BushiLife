using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Weight
{
    #region SubClasses

    [System.Serializable]
    public class Map : Dictionary<string, Weight>
    {

        public string defaultOperator = "Add";

        public static Map Empty
        {
            get
            {
                return new Map();
            }
        }

        public Map(params object[] args)
        {
            PushMany(args);
        }

        public Map()
        {
        }

        public void PushMany(params object[] args)
        {
            PushMany("", 1f, new Queue<object>(args));
        }

        private void PushMany(string name, Weight weight, Queue<object> args)
        {
            Push(name, weight);

            if (args.Peek().IsA("string"))
            {
                name = (string)args.Dequeue();
            }
            else
            {
                weight = (Weight)args.Dequeue();
            }

            PushMany(name, weight, args);
        }

        public void Push(string name, Weight value)
        {
            if (name != "")
                this.Add(name, value);
        }

        public void Push(string name)
        {
            if (name != "")
                this.Add(name, 1f);
        }

        public void Apply(string name, Weight value, string strOperator = "Add")
        {
            if (name != "")
            {
                if (this.ContainsKey(name))
                {
                    this[name].Do(strOperator)(value);
                }
            }
        }
    }

    public class OperatorBox : List<string>
    {
        public string positive
        {
            get
            {
                return this[0];
            }
            set
            {
                this[0] = value;
            }
        }
        public string negative
        {
            get
            {
                return this[negativeIndex];
            }
            set
            {
                this[negativeIndex] = value;
            }
        }

        private int negativeIndex
        {
            get
            {
                return this.Count - 1;
            }
        }

        public OperatorBox(params string[] names) : base(names)
        {
        }

        public bool TryDo(Weight self, Weight other, int index = 0)
        {

        }

        public void Do(Weight self, Weight other, int index=0)
        {
            The.Result(
                    self, 
                    (System.Reflection.MethodInfo)The.Method(this[index], self.GetType()), 
                    (float)other
                );
        }

        public string Opposite(string name)
        {
            return this.Find(value => name == value);
        }
    }

    #endregion

    public float value;

	public delegate void Operator(float weight);

    public static List<OperatorBox> operators = null;

	public Weight(float value)
	{
		this.Init(value);
	}

    static Weight()
    {
        operators = new List<OperatorBox>() {
            new OperatorBox("Add", "Remove"),
            new OperatorBox("Multiply", "Divide")
        };
    }

	/// <summary>
	/// Credit: http://stackoverflow.com/a/11065781
	/// </summary>
	/// <param name="weight"></param>
	public static implicit operator float(Weight weight)
	{
		return weight.value;
    }

    public virtual bool IsEmpty
    {
        get
        {
            return true;
        }
    }

    /// <summary>
    /// Credit: http://stackoverflow.com/a/11065781
    /// </summary>
    /// <param name="weight"></param>
    public static implicit operator Weight(float weight)
	{
		return new Weight(weight);
	}

	public Operator Do(string operation)
	{
		return operations[operation].function;
	}

	public static bool operator >(Weight left, Weight right)
	{
		return (float)left > (float)right;
	}

	public static bool operator <(Weight left, Weight right)
	{
		return (float)left < (float)right;
	}

	public void Init(float value)
	{
		this.value = value;

		operations = new Dictionary<string, Operator>()
		{
			{  },
			{ "Remove", this.Remove },
			{ "Scale", this.Scale },
			{ "Distance", this.Distance }
		};


	}

	public float Inverse
	{
		get
		{
			return new Weight(1f / this.value);
		}
	}

    public string InverseOperation(string operation)
    {
        return operations.ind
    }

	public void Scale(float weight)
	{
		this.value *= weight;
	}

	public void Add(float weight)
	{
		this.value += weight;
	}

	public void Remove(float weight)
	{
		this.Add(-weight);
	}

	public void Distance(float weight)
	{
		float result = Mathf.Abs(this.value - weight);

		value = result;
	}

	public Weight Scaled(float scale)
	{
		var result = this.Clone();

		result.Scale(scale);

		return result;
	}
}
