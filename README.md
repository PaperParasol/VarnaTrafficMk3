# Bus Routes
A WPF application providing bus routes in a town (Varna) in Bulgaria.
> [!NOTE]
> The Assembly name is the one that was used during the very early stages of this app's development. Due to serialization issues with the file `lineToStopsListData`,
> it cannot be changed to a new one.

## Credits and contribution
The class `Route` was written by Ognyan Arsov ([GitHub](https://github.com/ognyanarsov), [Instagram](https://www.instagram.com/ogiarsov?utm_source=ig_web_button_share_sheet&igsh=ZDNlZDc0MzIxNw==)). This project is published with his explicit permission.

## App Interface
The Application starts by default in fullscreen. A toggle fullscreen button can be found in the upper right corner.
The three tabs can be seen below. Bus Lines is the primary one, where most of the user interaction takes place. Admins and Edit 
are used to alter the list of lines and their bus stations.


![Main Page Interface](/WpfMapControls/mainwindow.png)

Start location `A` is set with a mouse right-click and an end location `B` is set with a double-click. 
The `Calculate` button is used to obtain an optimal bus route from point `A` to point `B`. 
> [!NOTE]
> Bus Stop names are currently in Bulgarian

The `Start` area shows the starting stop, highlighted with green on the map. Similarly for `Final`.
All Bus Stops, which the route goes through, are displayed on the map and in a list box on the left.
The `Data` area displays the name of the bus stop, which is currently being hovered on on the map.
Additionally, a blue line is displayed, where the bus would go.
A button `Map Mode` toggles the map between street mode and satelite mode.


![Demo Route](/WpfMapControls/routedemo.png)

## Known issues
The blue route line is sometimes displayed incorrectly, indicating that the bus should take weird U-turns and much longer ways, although
bus stops along the way are displayed properly.
