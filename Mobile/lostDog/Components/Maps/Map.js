import * as React from 'react';
import { View, StyleSheet,Text,TextInput,Dimensions,TouchableOpacity,Image } from 'react-native';
import MapView from 'react-native-maps';

const {width, height} = Dimensions.get("screen")


export default class Map extends React.Component {

  state={

  }

  render(){
    return(
        <View style={styles.content}>
          <Text>Hello I'm the map component</Text>
          <MapView
            initialRegion={{
            latitude: 37.78825,
            longitude: -122.4324,
            latitudeDelta: 0.0922,
            longitudeDelta: 0.0421,
            }}
        />
        </View>
  )
  }
}


const styles = StyleSheet.create({
    content: {
        marginHorizontal: 30,
        height: '100%',
        alignSelf: 'center',
        justifyContent: 'center',
        marginVertical: 'auto',
      },
});
