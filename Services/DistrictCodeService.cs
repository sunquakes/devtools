using System;
using System.Collections.Generic;
using System.Linq;
using DevTools.Models;

namespace DevTools.Services
{
    public class SearchResultItem
    {
        public string DisplayName { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
    }

    public class DistrictCodeService
    {
        private static readonly Lazy<DistrictCodeService> _instance = new Lazy<DistrictCodeService>(() => new DistrictCodeService());
        public static DistrictCodeService Instance => _instance.Value;

        public List<Province> Provinces { get; private set; } = new List<Province>();
        private Dictionary<string, AddressInfo> _codeMap = new Dictionary<string, AddressInfo>();
        private List<SearchResultItem> _allItems = new List<SearchResultItem>();

        private DistrictCodeService()
        {
            InitializeData();
        }

        private void InitializeData()
        {
            Provinces = new List<Province>
            {
                new Province
                {
                    Code = "110000",
                    Name = "北京市",
                    Cities = new List<City>
                    {
                        new City
                        {
                            Code = "110100",
                            Name = "北京市",
                            Districts = new List<District>
                            {
                                new District { Code = "110101", Name = "东城区" },
                                new District { Code = "110102", Name = "西城区" },
                                new District { Code = "110105", Name = "朝阳区" },
                                new District { Code = "110106", Name = "丰台区" },
                                new District { Code = "110107", Name = "石景山区" },
                                new District { Code = "110108", Name = "海淀区" },
                                new District { Code = "110109", Name = "门头沟区" },
                                new District { Code = "110111", Name = "房山区" },
                                new District { Code = "110112", Name = "通州区" },
                                new District { Code = "110113", Name = "顺义区" },
                                new District { Code = "110114", Name = "昌平区" },
                                new District { Code = "110115", Name = "大兴区" },
                                new District { Code = "110116", Name = "怀柔区" },
                                new District { Code = "110117", Name = "平谷区" },
                                new District { Code = "110118", Name = "密云区" },
                                new District { Code = "110119", Name = "延庆区" }
                            }
                        }
                    }
                },
                new Province
                {
                    Code = "120000",
                    Name = "天津市",
                    Cities = new List<City>
                    {
                        new City
                        {
                            Code = "120100",
                            Name = "天津市",
                            Districts = new List<District>
                            {
                                new District { Code = "120101", Name = "和平区" },
                                new District { Code = "120102", Name = "河东区" },
                                new District { Code = "120103", Name = "河西区" },
                                new District { Code = "120104", Name = "南开区" },
                                new District { Code = "120105", Name = "河北区" },
                                new District { Code = "120106", Name = "红桥区" },
                                new District { Code = "120110", Name = "东丽区" },
                                new District { Code = "120111", Name = "西青区" },
                                new District { Code = "120112", Name = "津南区" },
                                new District { Code = "120113", Name = "北辰区" },
                                new District { Code = "120114", Name = "武清区" },
                                new District { Code = "120115", Name = "宝坻区" },
                                new District { Code = "120116", Name = "滨海新区" },
                                new District { Code = "120117", Name = "宁河区" },
                                new District { Code = "120118", Name = "静海区" },
                                new District { Code = "120119", Name = "蓟州区" }
                            }
                        }
                    }
                },
                new Province
                {
                    Code = "310000",
                    Name = "上海市",
                    Cities = new List<City>
                    {
                        new City
                        {
                            Code = "310100",
                            Name = "上海市",
                            Districts = new List<District>
                            {
                                new District { Code = "310101", Name = "黄浦区" },
                                new District { Code = "310104", Name = "徐汇区" },
                                new District { Code = "310105", Name = "长宁区" },
                                new District { Code = "310106", Name = "静安区" },
                                new District { Code = "310107", Name = "普陀区" },
                                new District { Code = "310109", Name = "虹口区" },
                                new District { Code = "310110", Name = "杨浦区" },
                                new District { Code = "310112", Name = "闵行区" },
                                new District { Code = "310113", Name = "宝山区" },
                                new District { Code = "310114", Name = "嘉定区" },
                                new District { Code = "310115", Name = "浦东新区" },
                                new District { Code = "310116", Name = "金山区" },
                                new District { Code = "310117", Name = "松江区" },
                                new District { Code = "310118", Name = "青浦区" },
                                new District { Code = "310120", Name = "奉贤区" },
                                new District { Code = "310151", Name = "崇明区" }
                            }
                        }
                    }
                },
                new Province
                {
                    Code = "330000",
                    Name = "浙江省",
                    Cities = new List<City>
                    {
                        new City
                        {
                            Code = "330100",
                            Name = "杭州市",
                            Districts = new List<District>
                            {
                                new District { Code = "330102", Name = "上城区" },
                                new District { Code = "330105", Name = "拱墅区" },
                                new District { Code = "330106", Name = "西湖区" },
                                new District { Code = "330108", Name = "滨江区" },
                                new District { Code = "330109", Name = "萧山区" },
                                new District { Code = "330110", Name = "余杭区" },
                                new District { Code = "330111", Name = "富阳区" },
                                new District { Code = "330112", Name = "临安区" },
                                new District { Code = "330113", Name = "临平区" },
                                new District { Code = "330114", Name = "钱塘区" },
                                new District { Code = "330122", Name = "桐庐县" },
                                new District { Code = "330127", Name = "淳安县" },
                                new District { Code = "330182", Name = "建德市" }
                            }
                        },
                        new City
                        {
                            Code = "330200",
                            Name = "宁波市",
                            Districts = new List<District>
                            {
                                new District { Code = "330203", Name = "海曙区" },
                                new District { Code = "330205", Name = "江北区" },
                                new District { Code = "330206", Name = "北仑区" },
                                new District { Code = "330211", Name = "镇海区" },
                                new District { Code = "330212", Name = "鄞州区" },
                                new District { Code = "330213", Name = "奉化区" },
                                new District { Code = "330225", Name = "象山县" },
                                new District { Code = "330226", Name = "宁海县" },
                                new District { Code = "330281", Name = "余姚市" },
                                new District { Code = "330282", Name = "慈溪市" }
                            }
                        }
                    }
                },
                new Province
                {
                    Code = "440000",
                    Name = "广东省",
                    Cities = new List<City>
                    {
                        new City
                        {
                            Code = "440100",
                            Name = "广州市",
                            Districts = new List<District>
                            {
                                new District { Code = "440103", Name = "荔湾区" },
                                new District { Code = "440104", Name = "越秀区" },
                                new District { Code = "440105", Name = "海珠区" },
                                new District { Code = "440106", Name = "天河区" },
                                new District { Code = "440111", Name = "白云区" },
                                new District { Code = "440112", Name = "黄埔区" },
                                new District { Code = "440113", Name = "番禺区" },
                                new District { Code = "440114", Name = "花都区" },
                                new District { Code = "440115", Name = "南沙区" },
                                new District { Code = "440117", Name = "从化区" },
                                new District { Code = "440118", Name = "增城区" }
                            }
                        },
                        new City
                        {
                            Code = "440300",
                            Name = "深圳市",
                            Districts = new List<District>
                            {
                                new District { Code = "440303", Name = "罗湖区" },
                                new District { Code = "440304", Name = "福田区" },
                                new District { Code = "440305", Name = "南山区" },
                                new District { Code = "440306", Name = "宝安区" },
                                new District { Code = "440307", Name = "龙岗区" },
                                new District { Code = "440308", Name = "盐田区" },
                                new District { Code = "440309", Name = "龙华区" },
                                new District { Code = "440310", Name = "坪山区" },
                                new District { Code = "440311", Name = "光明区" },
                                new District { Code = "440312", Name = "大鹏新区" }
                            }
                        }
                    }
                }
            };

            BuildSearchIndex();
        }

        private void BuildSearchIndex()
        {
            foreach (var province in Provinces)
            {
                _allItems.Add(new SearchResultItem
                {
                    DisplayName = province.Name,
                    Code = province.Code
                });

                foreach (var city in province.Cities)
                {
                    _allItems.Add(new SearchResultItem
                    {
                        DisplayName = $"{province.Name}{city.Name}",
                        Code = city.Code
                    });

                    foreach (var district in city.Districts)
                    {
                        _allItems.Add(new SearchResultItem
                        {
                            DisplayName = $"{province.Name}{city.Name}{district.Name}",
                            Code = district.Code
                        });
                        _codeMap[district.Code] = new AddressInfo
                        {
                            Province = province.Name,
                            City = city.Name,
                            District = district.Name,
                            FullAddress = $"{province.Name}{city.Name}{district.Name}"
                        };
                    }
                }
            }
        }

        public List<SearchResultItem> SearchByKeyword(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return new List<SearchResultItem>();

            keyword = keyword.Trim();

            return _allItems
                .Where(x => x.DisplayName.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0 || 
                            x.Code.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0)
                .ToList();
        }

        public AddressInfo? QueryByCode(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                return null;

            code = code.Trim();
            _codeMap.TryGetValue(code, out var result);
            return result;
        }

        public string? GetDistrictCode(string provinceCode, string cityCode, string districtCode)
        {
            return districtCode;
        }
    }
}
