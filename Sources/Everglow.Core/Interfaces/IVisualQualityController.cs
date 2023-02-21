using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Everglow.Core.Enums;

namespace Everglow.Core.Interfaces;

public interface IVisualQualityController
{
	public VisualQuality Quality { get; }
	public bool High => Quality == VisualQuality.High;
	public bool Low => Quality == VisualQuality.Low;
}
