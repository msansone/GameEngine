#include "..\..\Headers\EngineCore\ColorRgba.hpp"

using namespace firemelon;

ColorRgba::ColorRgba(float r, float g, float b, float a)
{
	r_ = r;
	g_ = g;
	b_ = b;
	a_ = a;
}

ColorRgba::~ColorRgba()
{
}

boost::shared_ptr<ColorRgba> ColorRgba::createPy(float r, float g, float b, float a)
{
	PythonReleaseGil unlocker;

	return boost::shared_ptr<ColorRgba>(new ColorRgba(r, g, b, a)); 
}

float ColorRgba::getRPy()
{
	PythonReleaseGil unlocker;

	return getR();
}

float ColorRgba::getR()
{
	return r_;
}

void ColorRgba::setRPy(float value)
{
	PythonReleaseGil unlocker;

	setR(value);
}

void ColorRgba::setR(float value)
{
	r_ = value;
}

float ColorRgba::getGPy()
{
	PythonReleaseGil unlocker;

	return getG();
}

float ColorRgba::getG()
{
	return g_;
}

void ColorRgba::setGPy(float value)
{
	PythonReleaseGil unlocker;

	setG(value);
}

void ColorRgba::setG(float value)
{
	g_ = value;
}

float ColorRgba::getBPy()
{
	PythonReleaseGil unlocker;

	return getB();
}

float ColorRgba::getB()
{
	return b_;
}

void ColorRgba::setBPy(float value)
{
	PythonReleaseGil unlocker;

	setB(value);
}

void ColorRgba::setB(float value)
{
	b_ = value;
}

float ColorRgba::getAPy()
{
	PythonReleaseGil unlocker;

	return getA();
}

float ColorRgba::getA()
{
	return a_;
}

void ColorRgba::setAPy(float value)
{
	PythonReleaseGil unlocker;

	setA(value);
}

void ColorRgba::setA(float value)
{
	a_ = value;
}
