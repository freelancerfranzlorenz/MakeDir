/**
 *  Project:    makedir
 *  (c) Franz Lorenz, 2021
 */

using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

/**
 * Class of the GUI dialog
 */
public partial class Form1 : Form
{
    //
    public string   sDisplayName    = "MakeDir";
    public string   sCommandCall    = "cmd";
    public string   sToolDir        = "";
    public string   sMenuName       = "";
    public string   sMenuCmd        = "";

    /**
     * Constructor of the class.
     */
    public Form1()
    {
        InitializeComponent();
        sCommandCall = Environment.CommandLine + " %1";
        sToolDir     = Environment.CurrentDirectory;
        sMenuName    = "Directory\\shell\\" + sDisplayName;
        sMenuCmd     = sMenuName + "\\command";
    }

    /**
     * This callback function handles the installation
     * process of the tool.
     * @param sender      GUI object sender
     * @param e           events 
     */
    private void buttonInstall_Click(object sender, EventArgs e)
    {
        listBoxOutput.Items.Clear();                                            //clear the output view
        listBoxOutput.Items.Add("Starting Installation...");                    //output planned action
        if (addRegistry())
        {
            addPath(sToolDir);                                                    //add directory
        }
    }

    /**
     * This function adds
     */
    private bool addRegistry()
    {
        RegistryKey regMenu = null;
        RegistryKey regCmd = null;
        bool bRet = false;
        //
        listBoxOutput.Items.Add( "- write registry keys..." );
        try
        {
            regMenu = Registry.ClassesRoot.CreateSubKey(sMenuName);
            if (null != regMenu)
            {
                regMenu.SetValue("", sDisplayName);
                listBoxOutput.Items.Add("|- write regkey for '" + sDisplayName + "'");
                regCmd = Registry.ClassesRoot.CreateSubKey(sMenuCmd);
                if (null != regCmd)
                {
                    regCmd.SetValue("", sCommandCall);
                    listBoxOutput.Items.Add("|- write regkey for '" + sCommandCall + "'");
                    bRet = true;
                }
            }  //if( null != regMenu )
        }
        catch (Exception ex)
        {
            listBoxOutput.Items.Add("|- ERROR : please start this app as 'administrator'!");
        }
        finally
        {
            if (null != regMenu)
            {
                regMenu.Close();
            }
            if (null != regCmd)
            {
                regCmd.Close();
            }
        }
        return bRet;
    }   //addRegistry()

    /**
     * This function adds the path to the
     * @param   sDirToAdd   directory to add
     */
    private bool addPath(string sDirToAdd)
    {
        string sEnvPath = Environment.GetEnvironmentVariable("path", EnvironmentVariableTarget.Machine);
        bool bRet = false;
        //
        listBoxOutput.Items.Add("- update PATH environment...");
        //
        sDirToAdd = ";" + sDirToAdd + ";";                                      //prepare toolpath
        if (sEnvPath.IndexOf(sDirToAdd) < 0)                                    //is toolpath already in PATH?
        {                                                                       // no, then...
            sEnvPath += sDirToAdd;                                              // add toolpath
            try
            {
                Environment.SetEnvironmentVariable("path", sEnvPath, EnvironmentVariableTarget.Machine);
                listBoxOutput.Items.Add("|- add toolpath to PATH");
                bRet = true;
            }
            catch ( Exception ex )
            {
                listBoxOutput.Items.Add( "|- ERROR : please start this app as 'administrator'!");
            }
        }
        else
        {
            listBoxOutput.Items.Add("|- toolpath is already stored");
        }
        return bRet;
    }

    /**
     * This function will 
     */
    private void buttonRemove_Click(object sender, EventArgs e)
    {
        listBoxOutput.Items.Clear();
        listBoxOutput.Items.Add("Removing tool from your computer...");
        if (removeRegistry())
        {
            removePath(Environment.CurrentDirectory);
        }
    }

    /**
     * This function removes all registry entries of this app.
     * @return  bool        true - sucessfull removed
     */
    private bool removeRegistry()
    {
        bool bRet = false;
        //
        listBoxOutput.Items.Add("- remove all registry entries");
        try
        {
            Registry.ClassesRoot.DeleteSubKeyTree(sMenuName);
            listBoxOutput.Items.Add("|- removed all registry entries");
            bRet = true;
        }
        catch (Exception ex)
        {
            listBoxOutput.Items.Add("|- ERROR : please start this app as 'administrator'!");
        }
        return bRet;
    }

    /**
     * Remove path from the environment variable PATH
     * @param   sPath   path to remove
     */
    private bool removePath(string sDirToRemove)
    {
        string sEnvPath = Environment.GetEnvironmentVariable("path", EnvironmentVariableTarget.Machine);
        int nPos = 0;
        bool bRet = false;
        //
        listBoxOutput.Items.Add("- update PATH environment...");
        //
        sDirToRemove = ";" + sDirToRemove + ";";                                //prepare toolpath
        nPos = sEnvPath.IndexOf(sDirToRemove, 0, StringComparison.OrdinalIgnoreCase); //find toolpath
        if (nPos >= 0)                                                         //is toolpath already in PATH?
        {                                                                       // yes, then...
            string sEnvPath1 = sEnvPath.Substring(0, nPos);                     // get one part of the path
            nPos += sDirToRemove.Length;                                        // go to end of path
            string sEnvPath2 = sEnvPath.Substring(nPos);                        // get rest of path
            sEnvPath = sEnvPath1 + sEnvPath2;                                   // get new path
            try
            {
                Environment.SetEnvironmentVariable("path", sEnvPath, EnvironmentVariableTarget.Machine);
                listBoxOutput.Items.Add("|- remove toolpath to PATH");
                bRet = true;
            }
            catch (Exception ex)
            {
                listBoxOutput.Items.Add("|- ERROR : please start this app as 'administrator'!");
            }
        }
        else
        {
            listBoxOutput.Items.Add("|- toolpath is already removed");
        }
        return bRet;
    }

}
