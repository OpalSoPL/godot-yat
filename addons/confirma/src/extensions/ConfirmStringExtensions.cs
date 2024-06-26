using System;
using Confirma.Exceptions;

namespace Confirma.Extensions;

public static class ConfirmStringExtensions
{
	#region ConfirmEmpty
	public static void ConfirmEmpty(this string? actual, string? message = null)
	{
		if (string.IsNullOrEmpty(actual)) return;

		throw new ConfirmAssertException(
			message
			?? $"Expected empty string but found: {actual}"
		);
	}

	public static void ConfirmNotEmpty(this string? actual, string? message = null)
	{
		if (!string.IsNullOrEmpty(actual)) return;

		throw new ConfirmAssertException(
			message
			?? $"Expected non-empty string but found: {actual}"
		);
	}
	#endregion

	#region ConfirmContains
	public static void ConfirmContains(
		this string? actual,
		string expected,
		StringComparison comparisonType = StringComparison.OrdinalIgnoreCase,
		string? message = null
	)
	{
		if (actual?.Contains(expected, comparisonType) == true) return;

		throw new ConfirmAssertException(
			message
			?? $"Expected string to contain: {expected} but found: {actual}"
		);
	}

	public static void ConfirmNotContains(this string? actual, string expected, string? message = null)
	{
		if (actual?.Contains(expected) == false) return;

		throw new ConfirmAssertException(
			message
			?? $"Expected string to not contain: {expected} but found: {actual}"
		);
	}
	#endregion

	#region ConfirmStartsWith
	public static void ConfirmStartsWith(this string? actual, string expected, string? message = null)
	{
		if (actual?.StartsWith(expected) == true) return;

		throw new ConfirmAssertException(
			message
			?? $"Expected string to start with: {expected} but found: {actual}"
		);
	}

	public static void ConfirmNotStartsWith(this string? actual, string expected, string? message = null)
	{
		if (actual?.StartsWith(expected) == false) return;

		throw new ConfirmAssertException(
			message
			?? $"Expected string to not start with: {expected} but found: {actual}"
		);
	}
	#endregion

	#region ConfirmEndsWith
	public static void ConfirmEndsWith(this string? actual, string expected, string? message = null)
	{
		if (actual?.EndsWith(expected) == true) return;

		throw new ConfirmAssertException(
			message
			?? $"Expected string to end with: {expected} but found: {actual}"
		);
	}

	public static void ConfirmNotEndsWith(this string? actual, string expected, string? message = null)
	{
		if (actual?.EndsWith(expected) == false) return;

		throw new ConfirmAssertException(
			message
			?? $"Expected string to not end with: {expected} but found: {actual}"
		);
	}
	#endregion

	#region ConfirmHasLength
	public static void ConfirmHasLength(this string? actual, int expected, string? message = null)
	{
		if (actual?.Length == expected) return;

		throw new ConfirmAssertException(
			message
			?? $"Expected string to have length: {expected} but found: {actual?.Length}"
		);
	}

	public static void ConfirmNotHasLength(this string? actual, int expected, string? message = null)
	{
		if (actual?.Length != expected) return;

		throw new ConfirmAssertException(
			message
			?? $"Expected string to not have length: {expected} but found: {actual?.Length}"
		);
	}
	#endregion

	#region ConfirmEqualsCaseInsensitive
	public static void ConfirmEqualsCaseInsensitive(this string? actual, string expected, string? message = null)
	{
		if (string.Equals(actual, expected, StringComparison.OrdinalIgnoreCase)) return;

		throw new ConfirmAssertException(
			message
			?? $"Expected string to equal: {expected} but found: {actual}"
		);
	}

	public static void ConfirmNotEqualsCaseInsensitive(this string? actual, string expected, string? message = null)
	{
		if (!string.Equals(actual, expected, StringComparison.OrdinalIgnoreCase)) return;

		throw new ConfirmAssertException(
			message
			?? $"Expected string to not equal: {expected} but found: {actual}"
		);
	}
	#endregion
}
