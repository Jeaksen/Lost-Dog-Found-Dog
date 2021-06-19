import * as React from 'react';
import { View, StyleSheet,Text,TextInput,Dimensions,TouchableOpacity,Image, Alert } from 'react-native';
import MapView, { Callout, Circle, Marker } from 'react-native-maps';
import * as Location from 'expo-location'
import DogPin from '../Helpers/DogPin'
import GpsIcon from '../../Assets/gps.png';
import WrittingIcon from '../../Assets/writting.png';
import SkipIcon from '../../Assets/skip.png';

const {width, height} = Dimensions.get("screen")


export default class LocationPage extends React.Component {

    state={
        manuall: false,
        locationCity: "",
        locationDistrict: "",
        userPos: {latitude: 50.571077,longitude: 19.608812},
        region: 
        {
            /* Poland */
            latitude: 52,
		    longitude: 19,
		    latitudeDelta: 7,
		    longitudeDelta: 7
        },
    }

    componentDidMount(){
        this.locationInitProcess()
      }

      locationInitProcess = () => {
        this._getLocation()
        .then(location=>this.geoSuccess(location))
        .catch(error=> this.geoFailure(error));
      }
      _getLocation = async () =>{
        let { status } = await Location.requestForegroundPermissionsAsync();
          if (status !== 'granted') {
            this.permissionFailure()
            return null;
          }
          let location = await Location.getCurrentPositionAsync({});
          return location;
      }
      permissionFailure=(err)=>{
        Alert.alert(
            "Can't get permission for your location",
            "It looks like your app doesn't allow your location information to be obtained. The location is the most useful information to find a lost dog. Your data won't be used in any other process.",
            [
              {
                text: "Try again",
                onPress: () => this.locationInitProcess(),
                style: "default",
              },
              {
                text: "Set manually",
                onPress: () => this.setManually(),
                style: "default"
              },
              {
                text: "Skip",
                onPress: () => this.goToNext(),
                style: "cancel"
              },
              
            ],
            { cancelable: false });
      }
      geoSuccess =(position)=>{
        if (position==null) return;
        console.log(position)
        var transformLocation = {latitude: position.coords.latitude, longitude: position.coords.longitude}
        this.setState({userPos: transformLocation})
      }
      geoFailure =(err)=>{
        Alert.alert(
            "Can't get your location",
            "It looks like your app doesn't allow your location information to be obtained. The location is the most useful information to find a lost dog. Your data won't be used in any other process.",
            [
              {
                text: "Try again",
                onPress: () => this.locationInitProcess(),
                style: "default",
              },
              {
                text: "Skip",
                onPress: () => this.goToNext(),
                style: "cancel"
              },
              {
                text: "Set manually",
                onPress: () => this.goToNext(),
                style: "default"
              },
            ],
            { cancelable: false });
      }
      onMapRegionChange = (newRegion)=>{
        var newUserLoc = {latitude: newRegion.latitude,longitude: newRegion.longitude}
        this.setState({userPos: newUserLoc})
      }

    setManually=()=>{
        this.setState({manuall: true})
    }
    save=()=>{
        this.props.ParentRef.setLocation(
            {
                locationCity: this.state.userPos.latitude,
                locationDistrict: this.state.userPos.longitude,
            }
        )
        this.goToNext()
    }
    goToNext=()=>{
        this.props.ParentRef.moveToNext();
    }

  render(){
    return(
        <View style={styles.content}>
          <Text style={styles.Title}>Step 2/7 - Location</Text>  
            <View style={{ 
                marginTop: 5,
                height: "80%", 
                width: width*0.7,
                zIndex: -1, 
                borderRadius: 30, 
                borderWidth: 1, 
                borderColor: 'white', 
                overflow: 'hidden',
                alignItems: 'center',
                justifyContent: 'center'}}>

                <MapView 
                    style={{flex: 1, height: '100%', width: '100%', borderRadius: 10}} 
                    initialRegion={this.state.region}
                    onRegionChange={(x) => this.onMapRegionChange(x)}
                    //ref={ map => {currentMap = map }}
                    //region={props.region}
                    rotateEnabled={false}
                    loadingEnabled={true}>

                        <Marker coordinate={this.state.userPos} title={"Place where you last saw the dog"}>
                            <DogPin picture={this.props.userDogImage}/>
                        </Marker>
                    </MapView>                
            </View>
            <View style={{flexDirection: 'row', marginVertical: 10,}}>
              <TouchableOpacity style={styles.Button} onPress={() => this.locationInitProcess()}>
                  <Text style={styles.ButtonText} >Get my location</Text>
              </TouchableOpacity>
              <TouchableOpacity style={styles.Button} onPress={() => this.save()}>
                  <Text style={styles.ButtonText} >Mark it here</Text>
              </TouchableOpacity>
            </View>
        </View>
  )
  }
}

{
    /*
                <TextInput style={styles.inputtext} placeholder="City ..."      onChangeText={(x) => this.setState({locationCity: x})}/>
                <TextInput style={styles.inputtext} placeholder="District ..."  onChangeText={(x) => this.setState({locationDistrict: x})}/>
                <TouchableOpacity style={styles.Button} onPress={() => this.save()}>
                  <Image style={[styles.ButtonIcon, {marginLeft: '5%'}]} source={SkipIcon} />
                  <Text style={styles.ButtonText}>Continue</Text>
                </TouchableOpacity>
    */
}

const styles = StyleSheet.create({
    content: {
        height: '100%',
        margin: 5,
        alignSelf: 'center',
        marginVertical: 'auto',
    },
    Title:{
        fontSize: 20,
        marginTop: 5,
        alignSelf: 'center',
        color: '#99481f',
    },
    Button:{
        backgroundColor: '#feb26d',
        width: width*0.3,
        height: height*0.06,
        marginLeft: 'auto',
        marginRight: 'auto',
        flexDirection: 'row',
        alignContent: 'center',
        borderRadius: 15,
    },
    ButtonText:{
        marginTop: 'auto',
        marginBottom: 'auto',
        fontSize: 15,
        color: 'white',
        textAlign: 'center',
        width: '100%',
    },
    ButtonIcon:{
        width: 35,
        height:35,
        alignSelf: 'center',
    },
    inputtext: {
        fontSize: 16,
        height: 30,
        width: width*0.5,
        borderColor: '#000000',
        borderWidth: 1,
        borderRadius: 5,
        paddingLeft: 5,
        marginVertical: 10,
      },
      map: {
        width: 150,
        height: 100,
        flex: 1,
        borderRadius: 500,
        borderColor: '#000000',
        borderWidth: 1,
        backgroundColor: 'red',
        overflow: 'hidden',
      },
});
