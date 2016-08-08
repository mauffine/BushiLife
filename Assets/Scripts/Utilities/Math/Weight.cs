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

        public Map(List<Weight> values, params string[] names)
        {
            for (int i = 0; i < names.Length; i++)
            {
                this.Push(names[i], values[i % values.Count]);
            }
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
			if (args.Count == 0)
			{
				return;
			}

			if (!args.Peek().IsA("string"))
			{
				Push(name, weight);
				name = (string)args.Dequeue();
			}
			else
			{
				weight = (Weight)args.Dequeue();
				Push(name, weight);
			}

			PushMany(name, weight, args);
		}

		private void PushMany(Weight weight, string name, Queue<object> args)
		{ 
			if (args.Count == 0)
			{
				return;
			}

			if (!args.Peek().IsA("string"))
			{
				name = (string)args.Dequeue();
				Push(name, weight);
			}
			else
			{
				Push(name, weight);
				weight = (Weight)args.Dequeue();
			}

			PushMany(weight, name, args);
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

    public class Operator
    {
        public float power;
        private object function;

        public List<Mod> mods = null;

        public Operator(string function, float power)
        {
            this.power = power;

            this.function = Weight.Function(function);
        }

        public void Apply(float weight)
        {
            The.Result(
                    weight,
                    (System.Reflection.MethodInfo)function,
                        (float)weight,
                        power
                );
        }

        public class Mod : Operator
        {
            public Mod(string )
        }

        public class Box : Map
        {
            public string function;

            public float positive = 1f;
            public float negative = -1f;

            List<Box> children = new List<Box>();

            public Box(params object[] args) : base(args)
            {
            }

            public Box(params string[] names) : 
                base(
                    new List<Weight> { 1f, -1f }, 
                    names
                )
            {

            }

            public static Box Signed(string positive, string negative)
			{
				return new Box(positive, 1f, negative, -1f);
			}

            public Operator The(string operation)
            {
                return new Operator(operation, this[operation]);
            }

            public float Opposite(string name)
            {
                return Opposite(this[name]);
            }

            public static float Opposite(float power)
            {
                return power * -1f;
            }
        }

    }

    #endregion

    public float value;

	public delegate void OperatorFunction(float weight);

    public static List<Operator.Box> operators = null;

	public Weight(float value)
	{
		this.Init(value);
	}

    static Weight()
    {
        //operators = new List<OperatorBox>() {
            //Operator.Box.Signed("Add", "Remove"),
            //Operator.Box.Signed("Multiply", "Divide"),
        //};

        //var x = new Operator.Box("Add", "Remove", "Multiply", "Divide")
    }

    public static object Function(string name)
    {
        return The.Method(name, typeof(Weight));
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


	public void Do(string function, params object[] args)
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

		//operations = new Dictionary<string, Operator>()
		//{
			////{  },
			////{ "Remove", this.Remove },
			////{ "Scale", this.Scale },
			////{ "Distance", this.Distance }
		//};


	}

	public float Inverse
	{
		get
		{
			return new Weight(1f / this.value);
		}
	}

    public string Apply(float value, string operation, params string[] modifiers)
    {

    }

	public void Scale(float weight, float power=1f)
	{
        if (power == 1f)
        {
            this.value *= weight;
        }
        else if (power == -1f)
        {
            this.value /= weight;
        }
        else
        {
            this.value *= Mathf.Pow(weight, power);
        }
        
	}

	public void Join(float weight)
	{
		this.value += weight;
	}

	public void Remove(float weight)
	{
		//this.Add(-weight);
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
