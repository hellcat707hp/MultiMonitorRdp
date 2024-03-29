# MultiMonitorRdp
[![Build](https://github.com/hellcat707hp/MultiMonitorRdp/workflows/Build/badge.svg)](https://github.com/hellcat707hp/MultiMonitorRdp/actions/workflows/build.yml)

MultiMonitorRdp is a simple console program designed to make using Windows Remote Desktop client easier with multi-display setups. This program only exists because Windows tends to change which display numbers are which between restarts, sleeps, and even displays waking up.
___

## Usage
First, ensure you have a .rdp file available with all your existing settings. <br>
> You can create one by opening Remote Desktop Connection, expanding "Show Options", and choosing to save your connection settings at the bottom of the "General" tab.

Next, either download or build the program, then execute as follows:<br>
`MultiMonitorRdp.exe *monitors_to_use* *path_to_rdp_file*`

### Example

You want to use just the two rightmost monitors of your 3-monitor setup. (See the diagram in [Notes](#notes))

Execute the following command:<br>
`MultiMonitorRdp.exe "1,2" *path_to_rdp_file*`

## Notes
- I can't guarantee this is going to work well with every monitor setup out there. This has been (and will be) only tested with horizontal monitors that go left-to-right and touch at the borders (see diagram below), which is a fairly standard setup.
 ```
 Triple Monitor Example (Numbers are what MultiMonitorRdp expects, *not* the Windows-assigned numbers)
  ___________  ___________  ___________ 
 |           ||           ||           |
 |     0     ||     1     ||     2     |
 |___________||___________||___________| 
       |            |            |      
      
 ```

- Windows is very strange about display numbers even just waking up displays from sleep. Your remote session resizing randomly when waking up displays is totally "normal" and this program cannot fix that. You can usually just re-maximize the RDP window to fix it, or you may have to restart your remote session.
