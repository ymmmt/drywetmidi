﻿using Melanchall.DryWetMidi.Common;
using System;

namespace Melanchall.DryWetMidi.Smf.Interaction
{
    /// <summary>
    /// Represents metric length of an object expressed in hours, minutes and seconds.
    /// </summary>
    public sealed class MetricLength : ILength
    {
        #region Fields

        private readonly MetricTime _time;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="MetricLength"/>.
        /// </summary>
        public MetricLength()
            : this(0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MetricLength"/> with the specified
        /// number of microseconds.
        /// </summary>
        /// <param name="totalMicroseconds">Number of microseconds which represents metric length.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="totalMicroseconds"/> is negative.</exception>
        public MetricLength(long totalMicroseconds)
        {
            _time = new MetricTime(totalMicroseconds);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MetricLength"/> with the specified
        /// metric time using its total number of microseconds as the length.
        /// </summary>
        /// <param name="time">Metric time to initialize the <see cref="MetricLength"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="time"/> is null.</exception>
        public MetricLength(MetricTime time)
        {
            ThrowIfArgument.IsNull(nameof(time), time);

            _time = new MetricTime(time.TotalMicroseconds);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MetricLength"/> with the specified
        /// numbers of hours, minutes and seconds.
        /// </summary>
        /// <param name="hours">Number of hours.</param>
        /// <param name="minutes">Number of minutes.</param>
        /// <param name="seconds">Number of seconds.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="hours"/> is negative. -or-
        /// <paramref name="minutes"/> is negative. -or- <paramref name="seconds"/> is negative.</exception>
        public MetricLength(int hours, int minutes, int seconds)
            : this(hours, minutes, seconds, 0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MetricLength"/> with the specified
        /// numbers of hours, minutes, seconds and milliseconds.
        /// </summary>
        /// <param name="hours">Number of hours.</param>
        /// <param name="minutes">Number of minutes.</param>
        /// <param name="seconds">Number of seconds.</param>
        /// <param name="milliseconds">Number of milliseconds.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="hours"/> is negative. -or-
        /// <paramref name="minutes"/> is negative. -or- <paramref name="seconds"/> is negative. -or-
        /// <paramref name="milliseconds"/> is negative.</exception>
        public MetricLength(int hours, int minutes, int seconds, int milliseconds)
        {
            _time = new MetricTime(hours, minutes, seconds, milliseconds);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the value of the current <see cref="MetricLength"/> expressed in microseconds.
        /// </summary>
        public long TotalMicroseconds => _time.TotalMicroseconds;

        /// <summary>
        /// Gets the hours component of the length represented by the current <see cref="MetricLength"/>.
        /// </summary>
        public int Hours => _time.Hours;

        /// <summary>
        /// Gets the minutes component of the length represented by the current <see cref="MetricLength"/>.
        /// </summary>
        public int Minutes => _time.Minutes;

        /// <summary>
        /// Gets the seconds component of the length represented by the current <see cref="MetricLength"/>.
        /// </summary>
        public int Seconds => _time.Seconds;

        /// <summary>
        /// Gets the milliseconds component of the length represented by the current <see cref="MetricLength"/>.
        /// </summary>
        public int Milliseconds => _time.Milliseconds;

        #endregion

        #region Methods

        public static bool TryParse(string input, out MetricLength length)
        {
            return MetricLengthParser.TryParse(input, out length).Status == ParsingStatus.Parsed;
        }

        public static MetricLength Parse(string input)
        {
            var parsingResult = MetricLengthParser.TryParse(input, out var length);
            if (parsingResult.Status == ParsingStatus.Parsed)
                return length;

            throw parsingResult.Exception;
        }

        #endregion

        #region Operators

        /// <summary>
        /// Casts <see cref="TimeSpan"/> to <see cref="MetricLength"/>.
        /// </summary>
        /// <param name="timeSpan"><see cref="TimeSpan"/> to cast to <see cref="MetricLength"/>.</param>
        public static implicit operator MetricLength(TimeSpan timeSpan)
        {
            return new MetricLength(timeSpan);
        }

        /// <summary>
        /// Casts <see cref="MetricLength"/> to <see cref="TimeSpan"/>.
        /// </summary>
        /// <param name="length"><see cref="MetricLength"/> to cast to <see cref="TimeSpan"/>.</param>
        public static implicit operator TimeSpan(MetricLength length)
        {
            return length._time;
        }

        /// <summary>
        /// Determines if two <see cref="MetricLength"/> objects are equal.
        /// </summary>
        /// <param name="length1">The first <see cref="MetricLength"/> to compare.</param>
        /// <param name="length2">The second <see cref="MetricLength"/> to compare.</param>
        /// <returns>true if the lengths are equal, false otherwise.</returns>
        public static bool operator ==(MetricLength length1, MetricLength length2)
        {
            if (ReferenceEquals(length1, length2))
                return true;

            if (ReferenceEquals(null, length1) || ReferenceEquals(null, length2))
                return false;

            return length1.TotalMicroseconds == length2.TotalMicroseconds;
        }

        /// <summary>
        /// Determines if two <see cref="MetricLength"/> objects are not equal.
        /// </summary>
        /// <param name="length1">The first <see cref="MetricLength"/> to compare.</param>
        /// <param name="length2">The second <see cref="MetricLength"/> to compare.</param>
        /// <returns>false if the lengths are equal, true otherwise.</returns>
        public static bool operator !=(MetricLength length1, MetricLength length2)
        {
            return !(length1 == length2);
        }

        /// <summary>
        /// Adds two specified <see cref="MetricLength"/> instances.
        /// </summary>
        /// <param name="length1">The first length to add.</param>
        /// <param name="length2">The second length to add.</param>
        /// <returns>An object whose value is the sum of the values of <paramref name="length1"/> and
        /// <paramref name="length2"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="length1"/> is null. -or-
        /// <paramref name="length2"/> is null.</exception>
        public static MetricLength operator +(MetricLength length1, MetricLength length2)
        {
            ThrowIfArgument.IsNull(nameof(length1), length1);
            ThrowIfArgument.IsNull(nameof(length2), length2);

            return new MetricLength(length1.TotalMicroseconds + length2.TotalMicroseconds);
        }

        /// <summary>
        /// Subtracts a specified <see cref="MetricLength"/> from another specified <see cref="MetricLength"/>.
        /// </summary>
        /// <param name="length1">The minuend.</param>
        /// <param name="length2">The subtrahend.</param>
        /// <returns>An object whose value is the result of the value of <paramref name="length1"/> minus
        /// the value of <paramref name="length2"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="length1"/> is null. -or-
        /// <paramref name="length2"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="length1"/> is less than <paramref name="length2"/>.</exception>
        public static MetricLength operator -(MetricLength length1, MetricLength length2)
        {
            ThrowIfArgument.IsNull(nameof(length1), length1);
            ThrowIfArgument.IsNull(nameof(length2), length2);

            if (length1 < length2)
                throw new ArgumentException("First length is less than second one.", nameof(length1));

            return new MetricLength(length1.TotalMicroseconds - length2.TotalMicroseconds);
        }

        /// <summary>
        /// Indicates whether a specified <see cref="MetricLength"/> is less than another specified
        /// <see cref="MetricLength"/>.
        /// </summary>
        /// <param name="length1">The first length to compare.</param>
        /// <param name="length2">The second length to compare.</param>
        /// <returns>true if the value of <paramref name="length1"/> is less than the value of
        /// <paramref name="length2"/>; otherwise, false.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="length1"/> is null. -or-
        /// <paramref name="length2"/> is null.</exception>
        public static bool operator <(MetricLength length1, MetricLength length2)
        {
            ThrowIfArgument.IsNull(nameof(length1), length1);
            ThrowIfArgument.IsNull(nameof(length2), length2);

            return length1.TotalMicroseconds < length2.TotalMicroseconds;
        }

        /// <summary>
        /// Indicates whether a specified <see cref="MetricLength"/> is greater than another specified
        /// <see cref="MetricLength"/>.
        /// </summary>
        /// <param name="length1">The first length to compare.</param>
        /// <param name="length2">The second length to compare.</param>
        /// <returns>true if the value of <paramref name="length1"/> is greater than the value of
        /// <paramref name="length2"/>; otherwise, false.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="length1"/> is null. -or-
        /// <paramref name="length2"/> is null.</exception>
        public static bool operator >(MetricLength length1, MetricLength length2)
        {
            ThrowIfArgument.IsNull(nameof(length1), length1);
            ThrowIfArgument.IsNull(nameof(length2), length2);

            return length1.TotalMicroseconds > length2.TotalMicroseconds;
        }

        /// <summary>
        /// Indicates whether a specified <see cref="MetricLength"/> is less than or equal to
        /// another specified <see cref="MetricLength"/>.
        /// </summary>
        /// <param name="length1">The first length to compare.</param>
        /// <param name="length2">The second length to compare.</param>
        /// <returns>true if the value of <paramref name="length1"/> is less than or equal to the value of
        /// <paramref name="length2"/>; otherwise, false.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="length1"/> is null. -or-
        /// <paramref name="length2"/> is null.</exception>
        public static bool operator <=(MetricLength length1, MetricLength length2)
        {
            ThrowIfArgument.IsNull(nameof(length1), length1);
            ThrowIfArgument.IsNull(nameof(length2), length2);

            return length1.TotalMicroseconds <= length2.TotalMicroseconds;
        }

        /// <summary>
        /// Indicates whether a specified <see cref="MetricLength"/> is greater than or equal to
        /// another specified <see cref="MetricLength"/>.
        /// </summary>
        /// <param name="length1">The first length to compare.</param>
        /// <param name="length2">The second length to compare.</param>
        /// <returns>true if the value of <paramref name="length1"/> is greater than or equal to the value of
        /// <paramref name="length2"/>; otherwise, false.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="length1"/> is null. -or-
        /// <paramref name="length2"/> is null.</exception>
        public static bool operator >=(MetricLength length1, MetricLength length2)
        {
            ThrowIfArgument.IsNull(nameof(length1), length1);
            ThrowIfArgument.IsNull(nameof(length2), length2);

            return length1.TotalMicroseconds >= length2.TotalMicroseconds;
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>true if the specified object is equal to the current object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            return this == (obj as MetricLength);
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A 32-bit signed integer hash code.</returns>
        public override int GetHashCode()
        {
            return TotalMicroseconds.GetHashCode();
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return _time.ToString();
        }

        #endregion

        #region ILength

        public ILength Add(ILength length)
        {
            ThrowIfArgument.IsNull(nameof(length), length);

            var metricLength = length as MetricLength;
            return metricLength != null
                ? this + metricLength
                : LengthUtilities.Add(this, length);
        }

        public ILength Subtract(ILength length)
        {
            ThrowIfArgument.IsNull(nameof(length), length);

            var metricLength = length as MetricLength;
            return metricLength != null
                ? this - metricLength
                : LengthUtilities.Subtract(this, length);
        }

        public ILength Multiply(double multiplier)
        {
            ThrowIfArgument.IsNegative(nameof(multiplier), multiplier, "Multiplier is negative.");

            return new MetricLength((long)Math.Round(TotalMicroseconds * multiplier));
        }

        public ILength Divide(double divisor)
        {
            ThrowIfArgument.IsNegative(nameof(divisor), divisor, "Divisor is negative.");

            return new MetricLength((long)Math.Round(TotalMicroseconds / divisor));
        }

        #endregion
    }
}
