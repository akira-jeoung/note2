
// Lambda Expressions

	// 1. Expression Lambda
		
	delegate int del(int i);  
	static void Main(string[] args)  
	{  
		del myDelegate = x => x * x;  
		int j = myDelegate(5); //j = 25  
	}
	
	using System.Linq.Expressions;  	  
	namespace ConsoleApplication1  
	{  
		class Program  
		{  
			static void Main(string[] args)  
			{  
				Expression<del> myET = x => x * x;  
			}  
		}  
	}  
	
	
	public static IEnumerable<TSource> Where<TSource>(
	this IEnumerable<TSource> source,
	Func<TSource, bool> predicate
	)