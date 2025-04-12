using System;
using System.Collections.Generic;

namespace FVer.Tests {
    [TestClass]
    public sealed class TestFVer_Compare {
        private static readonly (string, int)[] testVersions = [
            ("02a", 5),
            ("preview03c", 2),
            ("alpha14d", 0),
            ("05f-pre", 9),
            ("beta112b", 1),
            ("special01r.1", 3),
            ("special02o.2-pre", 4),
            ("03s.4", 8),
            ("02z.5-pre", 6),
            ("02ab", 7),
        ];

        private readonly FVersion[] vers;
        private readonly FVersion[] sorted_vers;

        public TestFVer_Compare() {
            List<FVersion> vers = [];
            foreach ((string, int) verTest in testVersions) {
                FVersion ver = new(verTest.Item1);
                vers.Add(ver);
            }
            this.vers = [.. vers];
            FVersion[] sorted_vers = new FVersion[this.vers.Length];
            for (int i=0;i<sorted_vers.Length;i++) {
                sorted_vers[testVersions[i].Item2] = vers[i];
            }
            this.sorted_vers = sorted_vers;
        }

        [TestMethod]
        public void TestCompare_Order() {
            for (int i=0;i<sorted_vers.Length-1;i++) {
                int comp = sorted_vers[i].CompareTo(sorted_vers[i+1]);
                Assert.IsTrue(comp < 0, $"{sorted_vers[i]} is not less than {sorted_vers[i+1]}. res: {comp}");
            }
        }

        [TestMethod]
        public void TestCompare_Sort() {
            FVersion[] sort = new FVersion[vers.Length];
            Array.Copy(vers, sort, vers.Length);
            Array.Sort(sort);
            for (int i=0;i<vers.Length;i++) {
                Assert.IsTrue(sort[i] == sorted_vers[i]);
            }
        }
    }
}
