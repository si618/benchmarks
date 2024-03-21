window.BENCHMARK_DATA = {
  "lastUpdate": 1711011845947,
  "repoUrl": "https://github.com/si618/benchmarks",
  "entries": {
    "Benchmarks": [
      {
        "commit": {
          "author": {
            "email": "sshnug.si+github@gmail.com",
            "name": "Simon McKenna",
            "username": "si618"
          },
          "committer": {
            "email": "sshnug.si+github@gmail.com",
            "name": "Simon McKenna",
            "username": "si618"
          },
          "distinct": true,
          "id": "b34a77579510d01e2a897d3372c46afcfdb8aecc",
          "message": "Refactor",
          "timestamp": "2024-03-21T18:58:00+10:30",
          "tree_id": "2f65651a43f685fa55601689a39c36e80f81d1c6",
          "url": "https://github.com/si618/benchmarks/commit/b34a77579510d01e2a897d3372c46afcfdb8aecc"
        },
        "date": 1711011845045,
        "tool": "benchmarkdotnet",
        "benches": [
          {
            "name": "InsertGuidPrimaryKey_OnPostgres",
            "value": 48201986.29411763,
            "unit": "ns",
            "range": "± 961826.642354605"
          },
          {
            "name": "Benchmarks.GuidPrimaryKey.InsertGuidPrimaryKey_WithClusteredIndex_OnSqlServer(RowCount: 1000)",
            "value": 50018930.65833331,
            "unit": "ns",
            "range": "± 4163289.656605052"
          },
          {
            "name": "Benchmarks.GuidPrimaryKey.InsertGuidPrimaryKey_WithNonClusteredIndex_OnSqlServer(RowCount: 1000)",
            "value": 48525674.24242424,
            "unit": "ns",
            "range": "± 1187919.003398385"
          },
          {
            "name": "Benchmarks.GuidPrimaryKey.InsertGuidPrimaryKey_OnPostgres(RowCount: 10000)",
            "value": 346213557.46666664,
            "unit": "ns",
            "range": "± 6086463.646741817"
          },
          {
            "name": "Benchmarks.GuidPrimaryKey.InsertGuidPrimaryKey_WithClusteredIndex_OnSqlServer(RowCount: 10000)",
            "value": 446947331.5,
            "unit": "ns",
            "range": "± 2996630.8339389027"
          },
          {
            "name": "Benchmarks.GuidPrimaryKey.InsertGuidPrimaryKey_WithNonClusteredIndex_OnSqlServer(RowCount: 10000)",
            "value": 464172179.61538464,
            "unit": "ns",
            "range": "± 4354739.163136209"
          },
          {
            "name": "InsertThenHardDelete",
            "value": 90656548.73333333,
            "unit": "ns",
            "range": "± 2475453.1325594946"
          },
          {
            "name": "Benchmarks.SoftDelete.InsertThenSoftDeleteWithIndexFilter(RowCount: 1000, DbServer: Postgres)",
            "value": 134088408.97777778,
            "unit": "ns",
            "range": "± 4225506.65900386"
          },
          {
            "name": "Benchmarks.SoftDelete.InsertThenSoftDeleteWithoutIndexFilter(RowCount: 1000, DbServer: Postgres)",
            "value": 128922570.54878049,
            "unit": "ns",
            "range": "± 4091772.480034745"
          },
          {
            "name": "Benchmarks.SoftDelete.InsertThenListSoftDeleteWithIndexFilter(RowCount: 1000, DbServer: Postgres)",
            "value": 275581058.4275,
            "unit": "ns",
            "range": "± 108337137.83372904"
          },
          {
            "name": "Benchmarks.SoftDelete.InsertThenListSoftDeleteWithoutIndexFilter(RowCount: 1000, DbServer: Postgres)",
            "value": 306078812.4675,
            "unit": "ns",
            "range": "± 113545801.1337439"
          },
          {
            "name": "Benchmarks.SoftDelete.InsertThenHardDelete(RowCount: 1000, DbServer: SqlServer)",
            "value": 80200041.8525,
            "unit": "ns",
            "range": "± 9072576.098895593"
          },
          {
            "name": "Benchmarks.SoftDelete.InsertThenSoftDeleteWithIndexFilter(RowCount: 1000, DbServer: SqlServer)",
            "value": 136129849.51,
            "unit": "ns",
            "range": "± 11335007.36323263"
          },
          {
            "name": "Benchmarks.SoftDelete.InsertThenSoftDeleteWithoutIndexFilter(RowCount: 1000, DbServer: SqlServer)",
            "value": 154387889.945,
            "unit": "ns",
            "range": "± 18282849.049148355"
          },
          {
            "name": "Benchmarks.SoftDelete.InsertThenListSoftDeleteWithIndexFilter(RowCount: 1000, DbServer: SqlServer)",
            "value": 568777286.515,
            "unit": "ns",
            "range": "± 238819717.9288832"
          },
          {
            "name": "Benchmarks.SoftDelete.InsertThenListSoftDeleteWithoutIndexFilter(RowCount: 1000, DbServer: SqlServer)",
            "value": 588957893.6425,
            "unit": "ns",
            "range": "± 259074902.6867509"
          },
          {
            "name": "Benchmarks.SoftDelete.InsertThenHardDelete(RowCount: 10000, DbServer: Postgres)",
            "value": 834609515.9230769,
            "unit": "ns",
            "range": "± 5948323.380859762"
          },
          {
            "name": "Benchmarks.SoftDelete.InsertThenSoftDeleteWithIndexFilter(RowCount: 10000, DbServer: Postgres)",
            "value": 1266657737.2142856,
            "unit": "ns",
            "range": "± 15460003.7896328"
          },
          {
            "name": "Benchmarks.SoftDelete.InsertThenSoftDeleteWithoutIndexFilter(RowCount: 10000, DbServer: Postgres)",
            "value": 1262085262.0625,
            "unit": "ns",
            "range": "± 22909831.771998134"
          },
          {
            "name": "Benchmarks.SoftDelete.InsertThenListSoftDeleteWithIndexFilter(RowCount: 10000, DbServer: Postgres)",
            "value": 1149393937.35,
            "unit": "ns",
            "range": "± 282998428.43493"
          },
          {
            "name": "Benchmarks.SoftDelete.InsertThenListSoftDeleteWithoutIndexFilter(RowCount: 10000, DbServer: Postgres)",
            "value": 1110882416.24,
            "unit": "ns",
            "range": "± 259855498.19269595"
          },
          {
            "name": "Benchmarks.SoftDelete.InsertThenHardDelete(RowCount: 10000, DbServer: SqlServer)",
            "value": 591587986,
            "unit": "ns",
            "range": "± 6714735.201502765"
          },
          {
            "name": "Benchmarks.SoftDelete.InsertThenSoftDeleteWithIndexFilter(RowCount: 10000, DbServer: SqlServer)",
            "value": 1316917201.3076923,
            "unit": "ns",
            "range": "± 11972859.69712661"
          },
          {
            "name": "Benchmarks.SoftDelete.InsertThenSoftDeleteWithoutIndexFilter(RowCount: 10000, DbServer: SqlServer)",
            "value": 1169451453.142857,
            "unit": "ns",
            "range": "± 8756307.200363833"
          },
          {
            "name": "Benchmarks.SoftDelete.InsertThenListSoftDeleteWithIndexFilter(RowCount: 10000, DbServer: SqlServer)",
            "value": 1891008293.49,
            "unit": "ns",
            "range": "± 579548414.0238534"
          },
          {
            "name": "Benchmarks.SoftDelete.InsertThenListSoftDeleteWithoutIndexFilter(RowCount: 10000, DbServer: SqlServer)",
            "value": 1839667256.47,
            "unit": "ns",
            "range": "± 602503890.2775409"
          }
        ]
      }
    ]
  }
}