using System;
using System.Collections.Generic;

namespace FVer.Tests {
    [TestClass]
    public sealed class TestFVer_Parse {
        private static readonly (string, bool)[] testVersions = [
            ("02a", true),
            ("preview03c", true),
            ("alpha14d", true),
            ("14", false),
            ("05f-pre", true),
            ("beta112b", true),
            ("15.4", false),
            ("beta", false),
            ("special01r.1", true),
            ("4b", false),
            ("special02o.2-pre", true),
            ("a15O", false),
            ("03s.4", true),
            ("-02a", false),
            ("02z.5-pre", true),
            ("02ab", true),
        ];
        private static readonly FVersion[] expectedVersions = [
            new(2, 0, 0),
            new(3, 2, 0, "preview"),
            new(14, 3, 0, "alpha"),
            new(5, 5, 0, "", "pre"),
            new(112, 1, 0, "beta"),
            new(1, 17, 1, "special"),
            new(2, 14, 2, "special", "pre"),
            new(3, 18, 4),
            new(2, 25, 5, "", "pre"),
            new(2, 27)
        ];

        private readonly FVersion[] vers;

        public TestFVer_Parse() {
            bool failnull = false;
            try {
                FVersion _ = new("");
            } catch (ArgumentNullException) {
                failnull = true;
            }
            Assert.IsTrue(failnull, "Did not fail with a null string.");
            List<FVersion> _vers = [];
            foreach ((string, bool) verTest in testVersions) {
                if (!verTest.Item2) {
                    FVersion ver = null;
                    try {
                        ver = new FVersion(verTest.Item1);
                    } catch (FormatException) {}
                    catch (Exception e) {
                        throw new Exception($"For {verTest.Item1}: {e}");
                    }
                    Assert.IsNull(ver, $"Did not fail with malformed string \"{verTest.Item1}\"");
                }
                else {
                    try {
                        FVersion ver = new(verTest.Item1);
                        _vers.Add(ver);
                    } catch (Exception e) {
                        throw new Exception($"For {verTest.Item1}: {e}");
                    }
                }
            }
            vers = [.. _vers];
        }

        [TestMethod]
        public void TestParse_Match() {
            Assert.AreEqual(expectedVersions.Length, vers.Length, $"Parsed count mismatches expected count.");
            for (int i=0;i<vers.Length;i++) {
                bool res = vers[i] == expectedVersions[i];
                Assert.IsTrue(res, $"Value of parsed version {vers[i]} mismatches expected {expectedVersions[i]} internally.");
                int ver_hash = vers[i].GetHashCode();
                int expected_hash = expectedVersions[i].GetHashCode();
                bool hash_res = ver_hash == expected_hash;
                Assert.IsTrue(hash_res, $"Hash of parsed version {vers[i]} ({ver_hash}) mismatches expected {expectedVersions[i]} ({expected_hash}).");
                int compare_res = vers[i].CompareTo(expectedVersions[i]);
                Assert.IsTrue(compare_res == 0, $"Comparison of parsed version {vers[i]} is different from expected {expectedVersions[i]} ({compare_res}).");
            }
        }
    }
}
