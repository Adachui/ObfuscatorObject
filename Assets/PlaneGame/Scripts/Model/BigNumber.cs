using System;
using System.Globalization;
using UnityEngine;


public class BigNumber
{
	public BigNumber()
	{
		this.number = 0.0;
		this.units = 1;
	}
		
	public BigNumber(double num, int units)
	{
		this.number = num;
		this.units = units;
		if (this.units < 1) {
			this.units = 1;
		}
		this.BalanceUnits();
	}


	public BigNumber SetNumber(double num)
	{
		this.number = num;
		this.units = 1;
		this.BalanceUnits();
		return this;
	}

	public BigNumber(long num)
	{
		this.units = 1;
		if (num >= 1000000) {
			num /= 1000;
			this.units++;
		}
		this.number = num;
		this.BalanceUnits();
	}

	public BigNumber SetNumber(double num, int unit)
	{
		this.number = num;
		this.units = unit;
		if (this.units < 1) {
			this.units = 1;
		}
		this.BalanceUnits();
		return this;
	}


	public BigNumber SetNumber(BigNumber num)
	{
		this.number = num.number;
		this.units = num.units;
		if (this.units < 1) {
			this.units = 1;
		}
		return this;
	}


	public BigNumber SetIntegerNumber(BigNumber num)
	{
		this.number = (double)((int)num.number);
		this.units = num.units;
		if (this.units < 1) {
			this.units = 1;
		}
		return this;
	}


	public bool Equals(BigNumber value)
	{
		return this.units == value.units && (int)this.number == (int)value.number;
	}


	public bool IsBiggerThan(BigNumber currentBigNumber)
	{
		this.FixNumber();
		currentBigNumber.FixNumber();
		if (this.units == currentBigNumber.units) {
			
			return (this.number - currentBigNumber.number > 0 || Math.Abs (this.number - currentBigNumber.number) < 0.01);
		}
		return this.units >= currentBigNumber.units;
	}


	public bool IsBiggerAndNotEquals(BigNumber currentBigNumber)
	{
		this.FixNumber();
		currentBigNumber.FixNumber();
		if (this.units == currentBigNumber.units)
		{
			return this.number > currentBigNumber.number;
		}
		return this.units > currentBigNumber.units;
	}


	public BigNumber Add(BigNumber addNumber)
	{
		int num = Math.Abs(this.units - addNumber.units);
		if (this.units > addNumber.units)
		{
			this.number += addNumber.number * Math.Pow(0.001, (double)num);
		}
		else
		{
			double num2 = addNumber.number;
			num2 += this.number * Math.Pow(0.001, (double)num);
			this.number = num2;
			this.units = addNumber.units;
		}
		this.BalanceUnits();
		return this;
	}


	public BigNumber Add(double num)
	{
		if (this.units != 0)
		{
			num *= Math.Pow(0.001, (double)this.units);
		}
		this.number += num;
		this.BalanceUnits();
		return this;
	}


	public BigNumber Minus(BigNumber minusNumber)
	{
		if (this.units >= minusNumber.units)
		{
			int num = this.units - minusNumber.units;
			if (num <= 5)
			{
				this.number -= minusNumber.number * Math.Pow(0.001, (double)num);
			}
		}
		else
		{
			Debug.Log("Minus Error");
		}
		if (this.number < 0.0)
		{
			this.number = 0.0;
			this.units = 0;
		}
		this.FixNumber();
		return this;
	}
		
	private void BalanceUnits()
	{
		while (this.number > 1000000.0)
		{
			this.number /= 1000.0;
			this.units++;
		}
	}
		
	public double FixNumber()
	{
		while (this.number < 1000.0)
		{
			if (this.units <= 1)
			{
				break;
			}
			this.units--;
			this.number *= 1000.0;
		}
		return this.number;
	}

	public double FixNumberLowerThan1000()
	{
		while (this.number > 1000.0)
		{
			this.number /= 1000.0;
			this.units++;
		}
		return this.number;
	}


	public string GetUnit()
	{
		string empty = string.Empty;
        if (this.units > 0 && this.units <= DataManager.UnitConfigList.Count)
		{
			empty = DataManager.UnitConfigList[this.units - 1]["Part"];
		}
		return empty;
	}
		
	public override string ToString()
	{
		if (this.units == 0)
		{
			return this.number.ToString("#,0", CultureInfo.InvariantCulture);
		}
		//string str = this.FixNumber().ToString();
		string str = this.FixNumber().ToString("#,0", CultureInfo.InvariantCulture);
		return str + this.GetUnit();
	}
		
	public string ToInternationalizationString()
	{
		if (this.units < 1)
		{
			return this.GetInternationalizationNum();
		}
		return this.GetInternationalizationNum() + this.GetUnit();
	}
		
	private string GetInternationalizationNum()
	{
		if (this.number < 10.0)
		{
			return this.number.ToString("#,0.00", CultureInfo.InvariantCulture);
		}
		if (this.number >= 10.0 && this.number < 100.0)
		{
			return this.number.ToString("#,0.0", CultureInfo.InvariantCulture);
		}
		return this.number.ToString("#,0", CultureInfo.InvariantCulture);
	}

	public double number;

	public int units;
}
