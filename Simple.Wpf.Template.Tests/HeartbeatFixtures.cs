﻿namespace Simple.Wpf.Template.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Reactive;
    using Microsoft.Reactive.Testing;
    using NUnit.Framework;
    using Services;

    [TestFixture]
    public class HeartbeatFixtures
    {
        private TestScheduler _testScheduler;
        private MockSchedulerService _schedulerService;

        [SetUp]
        public void SetUp()
        {
            _testScheduler = new TestScheduler();
            _schedulerService = new MockSchedulerService(_testScheduler);
        }

        [Test]
        public void beats_regularly()
        {
            // ARRANGE
            var hearbeat = new HeartbeatService(TimeSpan.FromMilliseconds(200), _schedulerService);

            // ACT
            var beats = new List<Unit>();
            hearbeat.Listen.Subscribe(beats.Add);

            _testScheduler.AdvanceBy(TimeSpan.FromMilliseconds(450));

            // ASSERT
            Assert.That(beats, Is.Not.Empty);
            Assert.That(beats.Count, Is.EqualTo(2));
        }

        [Test]
        public void disposing_stops_the_heart_beating()
        {
            // ARRANGE
            var hearbeat = new HeartbeatService(TimeSpan.FromMilliseconds(200), _schedulerService);

            // ACT
            var beats = new List<Unit>();
            hearbeat.Listen.Subscribe(beats.Add);

            hearbeat.Dispose();

            _testScheduler.AdvanceBy(TimeSpan.FromMilliseconds(450));

            // ASSERT
            Assert.That(beats, Is.Empty);
        }
    }
}
