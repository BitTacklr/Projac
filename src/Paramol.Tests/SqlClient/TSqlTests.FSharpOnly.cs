#if FSHARP
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using NUnit.Framework;
using Paramol.SqlClient;
using Paramol.Tests.Framework;
using Microsoft.FSharp.Core;
using Microsoft.FSharp.Collections;

namespace Paramol.Tests.SqlClient
{
    public partial class TSqlTests
    {
        [Test]
        public void OptionalBigIntReturnsExpectedInstance()
        {
            Assert.That(TSql.BigInt(FSharpOption<long>.Some(123)), Is.EqualTo(new TSqlBigIntValue(123)));
        }

        [Test]
        public void BigIntReturnsExpectedInstance()
        {
            Assert.That(TSql.BigInt(123), Is.EqualTo(new TSqlBigIntValue(123)));
        }

        [Test]
        public void BigIntNoneReturnsExpectedInstance()
        {
            Assert.That(TSql.BigInt(FSharpOption<long>.None), Is.EqualTo(TSqlBigIntNullValue.Instance));
        }

        [Test]
        public void OptionalIntReturnsExpectedInstance()
        {
            Assert.That(TSql.Int(FSharpOption<int>.Some(123)), Is.EqualTo(new TSqlIntValue(123)));
        }

        [Test]
        public void IntReturnsExpectedInstance()
        {
            Assert.That(TSql.Int(123), Is.EqualTo(new TSqlIntValue(123)));
        }

        [Test]
        public void IntNoneReturnsExpectedInstance()
        {
            Assert.That(TSql.Int(FSharpOption<int>.None), Is.EqualTo(TSqlIntNullValue.Instance));
        }

        [Test]
        public void OptionalBitReturnsExpectedInstance()
        {
            Assert.That(TSql.Bit(FSharpOption<bool>.Some(true)), Is.EqualTo(new TSqlBitValue(true)));
        }

        [Test]
        public void BitReturnsExpectedInstance()
        {
            Assert.That(TSql.Bit(true), Is.EqualTo(new TSqlBitValue(true)));
        }

        [Test]
        public void BitNoneReturnsExpectedInstance()
        {
            Assert.That(TSql.Bit(FSharpOption<bool>.None), Is.EqualTo(TSqlBitNullValue.Instance));
        }

        [Test]
        public void OptionalUniqueIdentifierReturnsExpectedInstance()
        {
            Assert.That(TSql.UniqueIdentifier(FSharpOption<Guid>.Some(Guid.Empty)), Is.EqualTo(new TSqlUniqueIdentifierValue(Guid.Empty)));
        }

        [Test]
        public void UniqueIdentifierReturnsExpectedInstance()
        {
            Assert.That(TSql.UniqueIdentifier(Guid.Empty), Is.EqualTo(new TSqlUniqueIdentifierValue(Guid.Empty)));
        }

        [Test]
        public void UniqueIdentifierNoneReturnsExpectedInstance()
        {
            Assert.That(TSql.UniqueIdentifier(FSharpOption<Guid>.None), Is.EqualTo(TSqlUniqueIdentifierNullValue.Instance));
        }

        [Test]
        public void OptionalDateTimeOffsetReturnsExpectedInstance()
        {
            var value = DateTimeOffset.UtcNow;
            Assert.That(TSql.DateTimeOffset(FSharpOption<DateTimeOffset>.Some(value)), Is.EqualTo(new TSqlDateTimeOffsetValue(value)));
        }

        [Test]
        public void DateTimeOffsetReturnsExpectedInstance()
        {
            var value = DateTimeOffset.UtcNow;
            Assert.That(TSql.DateTimeOffset(value), Is.EqualTo(new TSqlDateTimeOffsetValue(value)));
        }

        [Test]
        public void DateTimeOffsetNoneReturnsExpectedInstance()
        {
            Assert.That(TSql.DateTimeOffset(FSharpOption<DateTimeOffset>.None), Is.EqualTo(TSqlDateTimeOffsetNullValue.Instance));
        }
    }
}
#endif