
#region Copyright (c) 2015 KEngine / Kelly <http://github.com/mr-kelly>, All rights reserved.

// KEngine - Asset Bundle framework for Unity3D
// ===================================
// 
// Author:  Kelly
// Email: 23110388@qq.com
// Github: https://github.com/mr-kelly/KEngine
// 
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 3.0 of the License, or (at your option) any later version.
// 
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public
// License along with this library.

#endregion

// This file is auto generated by KSettingModuleEditor.cs!
// Don't manipulate me!

using System.Collections;
using System.Collections.Generic;
using CosmosTable;
using KEngine;
using KEngine.Modules;
namespace AppSettings
{
	/// <summary>
    /// All settings list here, so you can reload all settings manully from the list.
	/// </summary>
    public partial class SettingsDefine
    {
        private static IReloadableSettings[] _settingsList;
        public static IReloadableSettings[] SettingsList
        {
            get
            {
                if (_settingsList == null)
                {
                    _settingsList = new IReloadableSettings[]
                    { 
                        AppConfigSettings.GetInstance(),
                        ExampleSettings.GetInstance(),
                        SubdirExample2Settings.GetInstance(),
                        SubdirSettings.GetInstance(),
                        SubdirSubSubDirExample3Settings.GetInstance(),
                    };
                }
                return _settingsList;
            }
        }
#if UNITY_EDITOR
        static bool HasAllReload = false;
        [UnityEditor.MenuItem("KEngine/Settings/Try Reload All Settings Code")]
	    public static void AllSettingsReload()
	    {
	        for (var i = 0; i < SettingsList.Length; i++)
	        {
	            var settings = SettingsList[i];
                if (HasAllReload) settings.ReloadAll();
                HasAllReload = true;

	            KLogger.Log("Reload settings: {0}, Row Count: {1}", settings.GetType(), settings.Count);

	        }
	    }

#endif
    }


	/// <summary>
	/// Auto Generate for Tab File: "AppConfig+Category.bytes", "AppConfig+Category2.bytes", "AppConfig.bytes"
    /// No use of generic and reflection, for better performance,  less IL code generating
	/// </summary>>
    public partial class AppConfigSettings : IReloadableSettings
    {
		public static readonly string[] TabFilePaths = 
        {
            "AppConfig+Category.bytes", "AppConfig+Category2.bytes", "AppConfig.bytes"
        };
        static AppConfigSettings _instance;
        Dictionary<string, AppConfigSetting> _dict = new Dictionary<string, AppConfigSetting>();

        /// <summary>
        /// Trigger delegate when reload the Settings
        /// </summary>>
	    public static System.Action OnReload;

        /// <summary>
        /// Constructor, just reload(init)
        /// When Unity Editor mode, will watch the file modification and auto reload
        /// </summary>
	    private AppConfigSettings()
	    {
        }

        /// <summary>
        /// Get the singleton
        /// </summary>
        /// <returns></returns>
	    public static AppConfigSettings GetInstance()
	    {
            if (_instance == null) 
            {
                _instance = new AppConfigSettings();

                _instance._ReloadAll(true);
    #if UNITY_EDITOR
                if (SettingModule.IsFileSystemMode)
                {
                    for (var j = 0; j < TabFilePaths.Length; j++)
                    {
                        var tabFilePath = TabFilePaths[j];
                        SettingModule.WatchSetting(tabFilePath, (path) =>
                        {
                            if (path.Replace("\\", "/").EndsWith(path))
                            {
                                _instance.ReloadAll();
                                KLogger.LogConsole_MultiThread("Reload success! -> " + path);
                            }
                        });
                    }

                }
    #endif
            }
	        return _instance;
	    }
        
        public int Count
        {
            get
            {
                return _dict.Count;
            }
        }

        /// <summary>
        /// Do reload the setting file: AppConfig, no exception when duplicate primary key
        /// </summary>
        public void ReloadAll()
        {
            _ReloadAll(false);
        }

        /// <summary>
        /// Do reload the setting file: AppConfig
        /// </summary>
	    void _ReloadAll(bool throwWhenDuplicatePrimaryKey)
        {
            for (var j = 0; j < TabFilePaths.Length; j++)
            {
                var tabFilePath = TabFilePaths[j];
                using (var tableFile = SettingModule.Get(tabFilePath, false))
                {
                    foreach (var row in tableFile)
                    {
                        var pk = AppConfigSetting.ParsePrimaryKey(row);
                        AppConfigSetting setting;
                        if (!_dict.TryGetValue(pk, out setting))
                        {
                            setting = new AppConfigSetting(row);
                            _dict[setting.Id] = setting;
                        }
                        else 
                        {
                            if (throwWhenDuplicatePrimaryKey) throw new System.Exception(string.Format("DuplicateKey, Class: {0}, File: {1}, Key: {2}", this.GetType().Name, tabFilePath, pk));
                            else setting.Reload(row);
                        }
                    }
                }
            }

	        if (OnReload != null)
	        {
	            OnReload();
	        }
        }

	    /// <summary>
        /// foreachable enumerable: AppConfig
        /// </summary>
        public static IEnumerable GetAll()
        {
            foreach (var row in GetInstance()._dict.Values)
            {
                yield return row;
            }
        }

        /// <summary>
        /// GetEnumerator for `MoveNext`: AppConfig
        /// </summary> 
	    public static IEnumerator GetEnumerator()
	    {
	        return GetInstance()._dict.Values.GetEnumerator();
	    }
         
	    /// <summary>
        /// Get class by primary key: AppConfig
        /// </summary>
        public static AppConfigSetting Get(string primaryKey)
        {
            AppConfigSetting setting;
            if (GetInstance()._dict.TryGetValue(primaryKey, out setting)) return setting;
            return null;
        }

        // ========= CustomExtraString begin ===========
        
        // ========= CustomExtraString end ===========
    }

	/// <summary>
	/// Auto Generate for Tab File: "AppConfig+Category.bytes", "AppConfig+Category2.bytes", "AppConfig.bytes"
    /// Singleton class for less memory use
	/// </summary>
	public partial class AppConfigSetting : TableRowParser
	{
		
        /// <summary>
        /// ID Column/编号/主键
        /// </summary>
        public string Id { get; private set;}
        
        /// <summary>
        /// Name/名字
        /// </summary>
        public string Value { get; private set;}
        

        internal AppConfigSetting(TableRow row)
        {
            Reload(row);
        }

        internal void Reload(TableRow row)
        { 
            Id = row.Get_string(row.Values[0], ""); 
            Value = row.Get_string(row.Values[1], ""); 
        }

        /// <summary>
        /// Get PrimaryKey from a table row
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public static string ParsePrimaryKey(TableRow row)
        {
            var primaryKey = row.Get_string(row.Values[0], "");
            return primaryKey;
        }
	}

	/// <summary>
	/// Auto Generate for Tab File: "Example.bytes"
    /// No use of generic and reflection, for better performance,  less IL code generating
	/// </summary>>
    public partial class ExampleSettings : IReloadableSettings
    {
		public static readonly string[] TabFilePaths = 
        {
            "Example.bytes"
        };
        static ExampleSettings _instance;
        Dictionary<string, ExampleSetting> _dict = new Dictionary<string, ExampleSetting>();

        /// <summary>
        /// Trigger delegate when reload the Settings
        /// </summary>>
	    public static System.Action OnReload;

        /// <summary>
        /// Constructor, just reload(init)
        /// When Unity Editor mode, will watch the file modification and auto reload
        /// </summary>
	    private ExampleSettings()
	    {
        }

        /// <summary>
        /// Get the singleton
        /// </summary>
        /// <returns></returns>
	    public static ExampleSettings GetInstance()
	    {
            if (_instance == null) 
            {
                _instance = new ExampleSettings();

                _instance._ReloadAll(true);
    #if UNITY_EDITOR
                if (SettingModule.IsFileSystemMode)
                {
                    for (var j = 0; j < TabFilePaths.Length; j++)
                    {
                        var tabFilePath = TabFilePaths[j];
                        SettingModule.WatchSetting(tabFilePath, (path) =>
                        {
                            if (path.Replace("\\", "/").EndsWith(path))
                            {
                                _instance.ReloadAll();
                                KLogger.LogConsole_MultiThread("Reload success! -> " + path);
                            }
                        });
                    }

                }
    #endif
            }
	        return _instance;
	    }
        
        public int Count
        {
            get
            {
                return _dict.Count;
            }
        }

        /// <summary>
        /// Do reload the setting file: Example, no exception when duplicate primary key
        /// </summary>
        public void ReloadAll()
        {
            _ReloadAll(false);
        }

        /// <summary>
        /// Do reload the setting file: Example
        /// </summary>
	    void _ReloadAll(bool throwWhenDuplicatePrimaryKey)
        {
            for (var j = 0; j < TabFilePaths.Length; j++)
            {
                var tabFilePath = TabFilePaths[j];
                using (var tableFile = SettingModule.Get(tabFilePath, false))
                {
                    foreach (var row in tableFile)
                    {
                        var pk = ExampleSetting.ParsePrimaryKey(row);
                        ExampleSetting setting;
                        if (!_dict.TryGetValue(pk, out setting))
                        {
                            setting = new ExampleSetting(row);
                            _dict[setting.Id] = setting;
                        }
                        else 
                        {
                            if (throwWhenDuplicatePrimaryKey) throw new System.Exception(string.Format("DuplicateKey, Class: {0}, File: {1}, Key: {2}", this.GetType().Name, tabFilePath, pk));
                            else setting.Reload(row);
                        }
                    }
                }
            }

	        if (OnReload != null)
	        {
	            OnReload();
	        }
        }

	    /// <summary>
        /// foreachable enumerable: Example
        /// </summary>
        public static IEnumerable GetAll()
        {
            foreach (var row in GetInstance()._dict.Values)
            {
                yield return row;
            }
        }

        /// <summary>
        /// GetEnumerator for `MoveNext`: Example
        /// </summary> 
	    public static IEnumerator GetEnumerator()
	    {
	        return GetInstance()._dict.Values.GetEnumerator();
	    }
         
	    /// <summary>
        /// Get class by primary key: Example
        /// </summary>
        public static ExampleSetting Get(string primaryKey)
        {
            ExampleSetting setting;
            if (GetInstance()._dict.TryGetValue(primaryKey, out setting)) return setting;
            return null;
        }

        // ========= CustomExtraString begin ===========
        
        // ========= CustomExtraString end ===========
    }

	/// <summary>
	/// Auto Generate for Tab File: "Example.bytes"
    /// Singleton class for less memory use
	/// </summary>
	public partial class ExampleSetting : TableRowParser
	{
		
        /// <summary>
        /// ID Column/编号/主键
        /// </summary>
        public string Id { get; private set;}
        
        /// <summary>
        /// Name/名字
        /// </summary>
        public string Name { get; private set;}
        
        /// <summary>
        /// 用于组合成Id主键
        /// </summary>
        public string KeyString { get; private set;}
        
        /// <summary>
        /// 数据测试
        /// </summary>
        public int Number { get; private set;}
        
        /// <summary>
        /// ArrayTest/测试数组
        /// </summary>
        public string[] StrArray { get; private set;}
        
        /// <summary>
        /// 字典测试
        /// </summary>
        public Dictionary<string,int> StrIntMap { get; private set;}
        

        internal ExampleSetting(TableRow row)
        {
            Reload(row);
        }

        internal void Reload(TableRow row)
        { 
            Id = row.Get_string(row.Values[0], ""); 
            Name = row.Get_string(row.Values[1], ""); 
            KeyString = row.Get_string(row.Values[2], ""); 
            Number = row.Get_int(row.Values[3], ""); 
            StrArray = row.Get_string_array(row.Values[4], ""); 
            StrIntMap = row.Get_Dictionary_string_int(row.Values[5], ""); 
        }

        /// <summary>
        /// Get PrimaryKey from a table row
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public static string ParsePrimaryKey(TableRow row)
        {
            var primaryKey = row.Get_string(row.Values[0], "");
            return primaryKey;
        }
	}

	/// <summary>
	/// Auto Generate for Tab File: "Subdir/Example2.bytes"
    /// No use of generic and reflection, for better performance,  less IL code generating
	/// </summary>>
    public partial class SubdirExample2Settings : IReloadableSettings
    {
		public static readonly string[] TabFilePaths = 
        {
            "Subdir/Example2.bytes"
        };
        static SubdirExample2Settings _instance;
        Dictionary<int, SubdirExample2Setting> _dict = new Dictionary<int, SubdirExample2Setting>();

        /// <summary>
        /// Trigger delegate when reload the Settings
        /// </summary>>
	    public static System.Action OnReload;

        /// <summary>
        /// Constructor, just reload(init)
        /// When Unity Editor mode, will watch the file modification and auto reload
        /// </summary>
	    private SubdirExample2Settings()
	    {
        }

        /// <summary>
        /// Get the singleton
        /// </summary>
        /// <returns></returns>
	    public static SubdirExample2Settings GetInstance()
	    {
            if (_instance == null) 
            {
                _instance = new SubdirExample2Settings();

                _instance._ReloadAll(true);
    #if UNITY_EDITOR
                if (SettingModule.IsFileSystemMode)
                {
                    for (var j = 0; j < TabFilePaths.Length; j++)
                    {
                        var tabFilePath = TabFilePaths[j];
                        SettingModule.WatchSetting(tabFilePath, (path) =>
                        {
                            if (path.Replace("\\", "/").EndsWith(path))
                            {
                                _instance.ReloadAll();
                                KLogger.LogConsole_MultiThread("Reload success! -> " + path);
                            }
                        });
                    }

                }
    #endif
            }
	        return _instance;
	    }
        
        public int Count
        {
            get
            {
                return _dict.Count;
            }
        }

        /// <summary>
        /// Do reload the setting file: SubdirExample2, no exception when duplicate primary key
        /// </summary>
        public void ReloadAll()
        {
            _ReloadAll(false);
        }

        /// <summary>
        /// Do reload the setting file: SubdirExample2
        /// </summary>
	    void _ReloadAll(bool throwWhenDuplicatePrimaryKey)
        {
            for (var j = 0; j < TabFilePaths.Length; j++)
            {
                var tabFilePath = TabFilePaths[j];
                using (var tableFile = SettingModule.Get(tabFilePath, false))
                {
                    foreach (var row in tableFile)
                    {
                        var pk = SubdirExample2Setting.ParsePrimaryKey(row);
                        SubdirExample2Setting setting;
                        if (!_dict.TryGetValue(pk, out setting))
                        {
                            setting = new SubdirExample2Setting(row);
                            _dict[setting.Id] = setting;
                        }
                        else 
                        {
                            if (throwWhenDuplicatePrimaryKey) throw new System.Exception(string.Format("DuplicateKey, Class: {0}, File: {1}, Key: {2}", this.GetType().Name, tabFilePath, pk));
                            else setting.Reload(row);
                        }
                    }
                }
            }

	        if (OnReload != null)
	        {
	            OnReload();
	        }
        }

	    /// <summary>
        /// foreachable enumerable: SubdirExample2
        /// </summary>
        public static IEnumerable GetAll()
        {
            foreach (var row in GetInstance()._dict.Values)
            {
                yield return row;
            }
        }

        /// <summary>
        /// GetEnumerator for `MoveNext`: SubdirExample2
        /// </summary> 
	    public static IEnumerator GetEnumerator()
	    {
	        return GetInstance()._dict.Values.GetEnumerator();
	    }
         
	    /// <summary>
        /// Get class by primary key: SubdirExample2
        /// </summary>
        public static SubdirExample2Setting Get(int primaryKey)
        {
            SubdirExample2Setting setting;
            if (GetInstance()._dict.TryGetValue(primaryKey, out setting)) return setting;
            return null;
        }

        // ========= CustomExtraString begin ===========
        
        // ========= CustomExtraString end ===========
    }

	/// <summary>
	/// Auto Generate for Tab File: "Subdir/Example2.bytes"
    /// Singleton class for less memory use
	/// </summary>
	public partial class SubdirExample2Setting : TableRowParser
	{
		
        /// <summary>
        /// ID Column/编号/主键
        /// </summary>
        public int Id { get; private set;}
        
        /// <summary>
        /// Name/名字
        /// </summary>
        public string Name { get; private set;}
        

        internal SubdirExample2Setting(TableRow row)
        {
            Reload(row);
        }

        internal void Reload(TableRow row)
        { 
            Id = row.Get_int(row.Values[0], ""); 
            Name = row.Get_string(row.Values[1], ""); 
        }

        /// <summary>
        /// Get PrimaryKey from a table row
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public static int ParsePrimaryKey(TableRow row)
        {
            var primaryKey = row.Get_int(row.Values[0], "");
            return primaryKey;
        }
	}

	/// <summary>
	/// Auto Generate for Tab File: "Subdir/__.bytes"
    /// No use of generic and reflection, for better performance,  less IL code generating
	/// </summary>>
    public partial class SubdirSettings : IReloadableSettings
    {
		public static readonly string[] TabFilePaths = 
        {
            "Subdir/__.bytes"
        };
        static SubdirSettings _instance;
        Dictionary<string, SubdirSetting> _dict = new Dictionary<string, SubdirSetting>();

        /// <summary>
        /// Trigger delegate when reload the Settings
        /// </summary>>
	    public static System.Action OnReload;

        /// <summary>
        /// Constructor, just reload(init)
        /// When Unity Editor mode, will watch the file modification and auto reload
        /// </summary>
	    private SubdirSettings()
	    {
        }

        /// <summary>
        /// Get the singleton
        /// </summary>
        /// <returns></returns>
	    public static SubdirSettings GetInstance()
	    {
            if (_instance == null) 
            {
                _instance = new SubdirSettings();

                _instance._ReloadAll(true);
    #if UNITY_EDITOR
                if (SettingModule.IsFileSystemMode)
                {
                    for (var j = 0; j < TabFilePaths.Length; j++)
                    {
                        var tabFilePath = TabFilePaths[j];
                        SettingModule.WatchSetting(tabFilePath, (path) =>
                        {
                            if (path.Replace("\\", "/").EndsWith(path))
                            {
                                _instance.ReloadAll();
                                KLogger.LogConsole_MultiThread("Reload success! -> " + path);
                            }
                        });
                    }

                }
    #endif
            }
	        return _instance;
	    }
        
        public int Count
        {
            get
            {
                return _dict.Count;
            }
        }

        /// <summary>
        /// Do reload the setting file: Subdir, no exception when duplicate primary key
        /// </summary>
        public void ReloadAll()
        {
            _ReloadAll(false);
        }

        /// <summary>
        /// Do reload the setting file: Subdir
        /// </summary>
	    void _ReloadAll(bool throwWhenDuplicatePrimaryKey)
        {
            for (var j = 0; j < TabFilePaths.Length; j++)
            {
                var tabFilePath = TabFilePaths[j];
                using (var tableFile = SettingModule.Get(tabFilePath, false))
                {
                    foreach (var row in tableFile)
                    {
                        var pk = SubdirSetting.ParsePrimaryKey(row);
                        SubdirSetting setting;
                        if (!_dict.TryGetValue(pk, out setting))
                        {
                            setting = new SubdirSetting(row);
                            _dict[setting.Id] = setting;
                        }
                        else 
                        {
                            if (throwWhenDuplicatePrimaryKey) throw new System.Exception(string.Format("DuplicateKey, Class: {0}, File: {1}, Key: {2}", this.GetType().Name, tabFilePath, pk));
                            else setting.Reload(row);
                        }
                    }
                }
            }

	        if (OnReload != null)
	        {
	            OnReload();
	        }
        }

	    /// <summary>
        /// foreachable enumerable: Subdir
        /// </summary>
        public static IEnumerable GetAll()
        {
            foreach (var row in GetInstance()._dict.Values)
            {
                yield return row;
            }
        }

        /// <summary>
        /// GetEnumerator for `MoveNext`: Subdir
        /// </summary> 
	    public static IEnumerator GetEnumerator()
	    {
	        return GetInstance()._dict.Values.GetEnumerator();
	    }
         
	    /// <summary>
        /// Get class by primary key: Subdir
        /// </summary>
        public static SubdirSetting Get(string primaryKey)
        {
            SubdirSetting setting;
            if (GetInstance()._dict.TryGetValue(primaryKey, out setting)) return setting;
            return null;
        }

        // ========= CustomExtraString begin ===========
        
        // ========= CustomExtraString end ===========
    }

	/// <summary>
	/// Auto Generate for Tab File: "Subdir/__.bytes"
    /// Singleton class for less memory use
	/// </summary>
	public partial class SubdirSetting : TableRowParser
	{
		
        /// <summary>
        /// ID Column/编号/主键
        /// </summary>
        public string Id { get; private set;}
        
        /// <summary>
        /// Name/名字
        /// </summary>
        public string Name { get; private set;}
        

        internal SubdirSetting(TableRow row)
        {
            Reload(row);
        }

        internal void Reload(TableRow row)
        { 
            Id = row.Get_string(row.Values[0], ""); 
            Name = row.Get_string(row.Values[1], ""); 
        }

        /// <summary>
        /// Get PrimaryKey from a table row
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public static string ParsePrimaryKey(TableRow row)
        {
            var primaryKey = row.Get_string(row.Values[0], "");
            return primaryKey;
        }
	}

	/// <summary>
	/// Auto Generate for Tab File: "Subdir/SubSubDir/Example3.bytes"
    /// No use of generic and reflection, for better performance,  less IL code generating
	/// </summary>>
    public partial class SubdirSubSubDirExample3Settings : IReloadableSettings
    {
		public static readonly string[] TabFilePaths = 
        {
            "Subdir/SubSubDir/Example3.bytes"
        };
        static SubdirSubSubDirExample3Settings _instance;
        Dictionary<string, SubdirSubSubDirExample3Setting> _dict = new Dictionary<string, SubdirSubSubDirExample3Setting>();

        /// <summary>
        /// Trigger delegate when reload the Settings
        /// </summary>>
	    public static System.Action OnReload;

        /// <summary>
        /// Constructor, just reload(init)
        /// When Unity Editor mode, will watch the file modification and auto reload
        /// </summary>
	    private SubdirSubSubDirExample3Settings()
	    {
        }

        /// <summary>
        /// Get the singleton
        /// </summary>
        /// <returns></returns>
	    public static SubdirSubSubDirExample3Settings GetInstance()
	    {
            if (_instance == null) 
            {
                _instance = new SubdirSubSubDirExample3Settings();

                _instance._ReloadAll(true);
    #if UNITY_EDITOR
                if (SettingModule.IsFileSystemMode)
                {
                    for (var j = 0; j < TabFilePaths.Length; j++)
                    {
                        var tabFilePath = TabFilePaths[j];
                        SettingModule.WatchSetting(tabFilePath, (path) =>
                        {
                            if (path.Replace("\\", "/").EndsWith(path))
                            {
                                _instance.ReloadAll();
                                KLogger.LogConsole_MultiThread("Reload success! -> " + path);
                            }
                        });
                    }

                }
    #endif
            }
	        return _instance;
	    }
        
        public int Count
        {
            get
            {
                return _dict.Count;
            }
        }

        /// <summary>
        /// Do reload the setting file: SubdirSubSubDirExample3, no exception when duplicate primary key
        /// </summary>
        public void ReloadAll()
        {
            _ReloadAll(false);
        }

        /// <summary>
        /// Do reload the setting file: SubdirSubSubDirExample3
        /// </summary>
	    void _ReloadAll(bool throwWhenDuplicatePrimaryKey)
        {
            for (var j = 0; j < TabFilePaths.Length; j++)
            {
                var tabFilePath = TabFilePaths[j];
                using (var tableFile = SettingModule.Get(tabFilePath, false))
                {
                    foreach (var row in tableFile)
                    {
                        var pk = SubdirSubSubDirExample3Setting.ParsePrimaryKey(row);
                        SubdirSubSubDirExample3Setting setting;
                        if (!_dict.TryGetValue(pk, out setting))
                        {
                            setting = new SubdirSubSubDirExample3Setting(row);
                            _dict[setting.Id] = setting;
                        }
                        else 
                        {
                            if (throwWhenDuplicatePrimaryKey) throw new System.Exception(string.Format("DuplicateKey, Class: {0}, File: {1}, Key: {2}", this.GetType().Name, tabFilePath, pk));
                            else setting.Reload(row);
                        }
                    }
                }
            }

	        if (OnReload != null)
	        {
	            OnReload();
	        }
        }

	    /// <summary>
        /// foreachable enumerable: SubdirSubSubDirExample3
        /// </summary>
        public static IEnumerable GetAll()
        {
            foreach (var row in GetInstance()._dict.Values)
            {
                yield return row;
            }
        }

        /// <summary>
        /// GetEnumerator for `MoveNext`: SubdirSubSubDirExample3
        /// </summary> 
	    public static IEnumerator GetEnumerator()
	    {
	        return GetInstance()._dict.Values.GetEnumerator();
	    }
         
	    /// <summary>
        /// Get class by primary key: SubdirSubSubDirExample3
        /// </summary>
        public static SubdirSubSubDirExample3Setting Get(string primaryKey)
        {
            SubdirSubSubDirExample3Setting setting;
            if (GetInstance()._dict.TryGetValue(primaryKey, out setting)) return setting;
            return null;
        }

        // ========= CustomExtraString begin ===========
        
        // ========= CustomExtraString end ===========
    }

	/// <summary>
	/// Auto Generate for Tab File: "Subdir/SubSubDir/Example3.bytes"
    /// Singleton class for less memory use
	/// </summary>
	public partial class SubdirSubSubDirExample3Setting : TableRowParser
	{
		
        /// <summary>
        /// ID Column/编号/主键
        /// </summary>
        public string Id { get; private set;}
        
        /// <summary>
        /// Name/名字
        /// </summary>
        public string Name { get; private set;}
        

        internal SubdirSubSubDirExample3Setting(TableRow row)
        {
            Reload(row);
        }

        internal void Reload(TableRow row)
        { 
            Id = row.Get_string(row.Values[0], ""); 
            Name = row.Get_string(row.Values[1], ""); 
        }

        /// <summary>
        /// Get PrimaryKey from a table row
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public static string ParsePrimaryKey(TableRow row)
        {
            var primaryKey = row.Get_string(row.Values[0], "");
            return primaryKey;
        }
	}
 
}
