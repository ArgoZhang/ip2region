// Copyright 2025 The Ip2Region Authors. All rights reserved.
// Use of this source code is governed by a Apache2.0-style
// license that can be found in the LICENSE file.
// Updated by Argo Zhang <argo@live.ca> at 2026/02/13

namespace IP2RegionMaker.XDB;

public class Segment
{
    public uint StartIP { get; set; }

    public uint EndIP { get; set; }

    public string? Region { get; set; }

    public List<Segment> Split()
    {
        var tList = new List<Segment>();
        var sByte = (StartIP >> 24) & 0xFF;
        var eByte = (EndIP >> 24) & 0xFF;

        var nSip = StartIP;
        for (var i = sByte; i <= eByte; i++)
        {
            var sip = (i << 24) | (nSip & 0xFFFFFF);
            var eip = (i << 24) | 0xFFFFFF;

            if (eip < EndIP)
            {
                nSip = (i + 1) << 24;
            }
            else
            {
                eip = EndIP;
            }

            tList.Add(new Segment
            {
                StartIP = sip,
                EndIP = eip,
            });
        }

        var segList = new List<Segment>();

        foreach (var seg in tList)
        {
            var temp = seg.StartIP & 0xFF000000;
            nSip = seg.StartIP;

            sByte = (seg.StartIP >> 16) & 0xFF;
            eByte = (seg.EndIP >> 16) & 0xFF;

            for (var i = sByte; i <= eByte; i++)
            {
                var sip = temp | (i << 16) | (nSip & 0xFFFF);
                var eip = temp | (i << 16) | 0xFFFF;

                if (eip < seg.EndIP)
                {
                    nSip = 0;
                }
                else
                {
                    eip = seg.EndIP;
                }

                segList.Add(new Segment
                {
                    StartIP = sip,
                    EndIP = eip,
                    Region = Region,
                });
            }
        }
        return segList;
    }

    public override string ToString()
    {
        return $"{Util.UInt32ToIpAddress(StartIP)}|{Util.UInt32ToIpAddress(EndIP)}|{Region}";
    }
}
