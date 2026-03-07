using TrookSii.Types.Raw;

namespace TrookSii.Types.Blocks;

/*
==> this is an info.sii file from a save
SiiNunit
   {
   save_container : _nameless.21d.6a77.7920 {
    name: "Unnamed save 1"
    time: 187992
    file_time: 1722121022
    version: 80
    info_version: 1
    dependencies: 34
    dependencies[0]: "mod|promods-assets-v270|ProMods Assets Package"
    dependencies[1]: "mod|promods-model2-v270|ProMods Models Package 2"
    dependencies[2]: "mod|promods-model1-v270|ProMods Models Package 1"
    dependencies[3]: "mod|promods-media-v270|ProMods Media Package"
    dependencies[4]: "mod|promods-map-v270|ProMods Map Package"
    dependencies[5]: "mod|promods-dlcsupport-v270|ProMods DLC Support Package"
    dependencies[6]: "mod|promods-model3-v270|ProMods Models Package 3"
    dependencies[7]: "mod|promods-def-v270|ProMods Definition Package"
    dependencies[8]: "rdlc|eut2_actros_tuning|DLC - Actros Tuning Pack"
    dependencies[9]: "dlc|eut2_balkane|DLC - Road to the Black Sea"
    dependencies[10]: "dlc|eut2_balkanw|DLC - West Balkans"
    dependencies[11]: "dlc|eut2_balt|DLC - Beyond the Baltic Sea"
    dependencies[12]: "dlc|eut2_daf_21|DLC - DAF 2021"
    dependencies[13]: "rdlc|eut2_dafpack|DLC - XF Tuning Pack"
    dependencies[14]: "dlc|eut2_daf_xd|DLC - DAF XD"
    dependencies[15]: "dlc|eut2_east|DLC - Going East!"
    dependencies[16]: "dlc|eut2_fr|DLC - Vive la France !"
    dependencies[17]: "rdlc|eut2_heavy_cargo|DLC - Heavy Cargo Pack"
    dependencies[18]: "rdlc|eut2_schoch|DLC - HS-Schoch Tuning Pack"
    dependencies[19]: "dlc|eut2_iberia|DLC - Iberia"
    dependencies[20]: "dlc|eut2_it|DLC - Italia"
    dependencies[21]: "dlc|eut2_man_tgx|DLC - MAN TGX 2020"
    dependencies[22]: "rdlc|eut2_metallics|DLC - Metallic Paint Jobs"
    dependencies[23]: "rdlc|eut2_metallics2|DLC - Flip Paint Designs"
    dependencies[24]: "rdlc|eut2_mighty_griffin|DLC - Mighty Griffin Tuning Pack"
    dependencies[25]: "dlc|eut2_north|DLC - Scandinavia"
    dependencies[26]: "rdlc|eut2_oversize|DLC - Special Transport"
    dependencies[27]: "rdlc|eut2_raven|DLC - Raven Truck Design Pack"
    dependencies[28]: "rdlc|eut2_retecht|DLC - Renault Trucks E-Tech T"
    dependencies[29]: "rdlc|eut2_rims|DLC - Wheels Tuning Pack"
    dependencies[30]: "rdlc|eut2_rocket_league|DLC - Rocket League"
    dependencies[31]: "dlc|eut2_scans24e|DLC - Scania S 2024E"
    dependencies[32]: "rdlc|eut2_trailers|DLC - High Power Cargo Pack"
    dependencies[33]: "rdlc|eut2_fhpack|DLC - FH Pack"
    info_players_experience: 142722
    info_unlocked_recruitments: 20
    info_unlocked_dealers: 22
    info_visited_cities: 212
    info_money_account: 8305
    info_explored_ratio: &3e85dafa
   }
   
   }
   
==> this is a profile.sii file
SiiNunit
   {
   user_profile : _nameless.2a8.ce28.2ec0 {
    face: 130
    brand: scania_streamline
    map_path: "/map/europe.mbd"
    logo: logo_6
    company_name: "Tigers Haul Things"
    male: true
    cached_experience: 142722
    cached_distance: 89617
    user_data: 19
    user_data[0]: ""
    user_data[1]: ""
    user_data[2]: ""
    user_data[3]: ""
    user_data[4]: "0.261436"
    user_data[5]: 99
    user_data[6]: 20
    user_data[7]: 1
    user_data[8]: 4
    user_data[9]: 187392
    user_data[10]: 5333
    user_data[11]: "mercedes.actros"
    user_data[12]: "daf.xd:1,iveco.hiway:1,daf.xf_euro6:1,scania.s_2016:1,man.tgx_euro6:1,scania.r_2016:1,renault.premium:1,scania.streamline:1,renault.t:1,mercedes.actros2014:1,iveco.stralis:2,man.tgx_2020:1,volvo.fh16_2012:1,mercedes.actros:2,renault.magnum:1,daf.xf:1,daf.2021:1,man.tgx:1"
    user_data[13]: ""
    user_data[14]: ""
    user_data[15]: ""
    user_data[16]: 0
    user_data[17]: ""
    user_data[18]: ""
    active_mods: 2
    active_mods[0]: "mod_workshop_package.00000000C5D284B9|Realistic Economy ETS2 by Quper"
    active_mods[1]: "mod_workshop_package.00000000C827A72C|Remover Ferries by Quper"
    customization: 187992
    cached_stats: 20
    cached_stats[0]: 167
    cached_stats[1]: 166
    cached_stats[2]: 47
    cached_stats[3]: 39
    cached_stats[4]: 239
    cached_stats[5]: 33
    cached_stats[6]: 187
    cached_stats[7]: 250
    cached_stats[8]: 60
    cached_stats[9]: 187
    cached_stats[10]: 93
    cached_stats[11]: 23
    cached_stats[12]: 159
    cached_stats[13]: 147
    cached_stats[14]: 18
    cached_stats[15]: 181
    cached_stats[16]: 1
    cached_stats[17]: 206
    cached_stats[18]: 124
    cached_stats[19]: 15
    cached_discovery: 800
    cached_discovery[0]: 119
    cached_discovery[1]: 58
    cached_discovery[2]: 81
    cached_discovery[3]: 79
    ...more cached discovery
    version: 6
    online_user_name: ""
    online_password: ""
    profile_name: "Jovin Baizceil"
    creation_time: 1718640218
    save_time: 1752534441
   }
   
   }
   
 */

public class PlainBlock(string structName, string id, IList<PlainSiiProperty> properties)
{
    public string StructureName { get; } = structName;

    public BlockId Id { get; } = new(id);

    public IDictionary<string, PlainSiiProperty> Properties { get; } = properties.ToDictionary(p => p.Name, p => p);
}