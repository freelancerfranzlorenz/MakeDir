/**
 *  Project:    makedir
 *  (c) Franz Lorenz, 2021
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;    //add directory

namespace makedir
{
    static class Program
    {

        /**
         * This list contains the directories, which
         * must be created by the tool.
         */
        static List<string> m_aListDirs = new List<string>();
        static String m_sBaseDir = "";

        /**
         * This function reads the configuration file
         * of the tool in the internal structure in aListDirs[].
         * @param   sDir    directory of the config file
         */
        static void readConfigFile( string sDir )
        {
            string sFile = sDir + "\\makedir.conf";                             //complete the filename of the configfile
            string[] asConfig = null;                                           //declare array for filecontent
            try
            {
                asConfig = File.ReadAllLines( sFile );                          //read all lines of the configfile
                foreach( string sLine in asConfig )                             //step line-by-line
                {                                                               // then...
                    sLine.Trim();                                               // delete spance before and behind the line
                    if( ! sLine.StartsWith( "#" ) )                             // is it a comment line?
                    {                                                           //  no, then...
                        if( sLine.StartsWith( "dir=" ) )                        //  directory line?
                        {                                                       //   yes, then...
                            m_aListDirs.Add(sLine.Substring(4));                //   get the directory entry
                        }
                        else if( sLine.StartsWith( "base=" ) )                  //  base naming? 
                        {                                                       //   yes, then...
                            m_sBaseDir = sLine.Substring(5);                    //   get the base dir
                        }
                    }   //if()
                }   //foreach()
            }
            catch( Exception )
            {
                Console.WriteLine("error : can't read config file '" + sFile + "'");
            }
        }

        /**
         * This function creates the directory structure
         * of the project.
         * @param   sBaseDir    base directory without finish \\
         */
        static void makeProjectDir( string sBaseDir )
        {
            sBaseDir += "\\";                                                   //add backslash
            if( m_sBaseDir.Length > 0 )                                         //get basedir naming?
            {                                                                   // yes, then...
                sBaseDir += m_sBaseDir;                                         //  add user basedir name
                String sYMD = DateTime.Now.ToString( "yyyyMMdd" );              //  get current date
                if( sBaseDir.IndexOf( "<YMD>" ) >= 0 )                          //  macro <YMD> found?
                {                                                               //   yes, then...
                    sBaseDir = sBaseDir.Replace( "<YMD>", sYMD );               //   replace macro with current date
                }
            }
            string sCurDir = sBaseDir;                                          //set the base directory
            try
            {
                Directory.CreateDirectory( sBaseDir );                          //create the base dir
                foreach( string sDir in m_aListDirs.ToArray() )                 //step through all readen directories
                {                                                               // then...
                    Console.WriteLine("info : create '"+sBaseDir+sDir+"'");     // echo the directory
                    Directory.CreateDirectory(sBaseDir+sDir );                  // create this directory
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error : can't create directory!");
            }
        }

        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if ( Environment.GetCommandLineArgs().Length > 1 )                  //more than one parameter given?
            {                                                                   // yes, then...
                string sToolDir = Environment.GetCommandLineArgs()[0];          // get the application call
                sToolDir = Path.GetDirectoryName(sToolDir);                     // extract only the path
                readConfigFile( sToolDir );                                     // read configuration file
                makeProjectDir( Environment.GetCommandLineArgs()[1] );          // make project directory structure
            }
            else                                                                //no parameter given
            {                                                                   // then...
                Application.EnableVisualStyles();                               //
                Application.SetCompatibleTextRenderingDefault(false);           //
                Application.Run( new Form1() );                                 // start GUI
            }
        }   //main()

    }

}
