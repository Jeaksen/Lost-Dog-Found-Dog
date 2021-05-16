import * as React from 'react';
import { View, StyleSheet,Text,TextInput,Dimensions,TouchableOpacity,Image } from 'react-native';
import ShelterIcon from '../Assets/animal-shelter.png'

const {width, height} = Dimensions.get("screen")


export default class ShelterDetails extends React.Component {
  constructor(props) {
    super(props);
   }

  render(){
    return(
      
      <View style={styles.content}>
        {
          this.props.item!=null?
            <View>
                <View style={{flexDirection: 'row', height: 100, marginHorizontal: 25}}>
                <Text style={{fontWeight: 'bold', fontSize: 30, marginVertical: 30}}>{this.props.item.name}</Text>
                </View>
                <View style={{margin: 50}}>
                  <Text style={{fontSize: 15}}><Text style={{fontWeight: 'bold'}}>Phone: </Text>{this.props.item.phoneNumber}</Text>
                  <Text style={{fontSize: 15}}><Text style={{fontWeight: 'bold'}}>Email: </Text>{this.props.item.email}</Text>
                  <Text style={{fontWeight: 'bold'}}>Address: </Text>
                  <Text style={{fontSize: 15}}><Text style={{fontWeight: 'bold'}}>City: </Text>{this.props.item.address.city}</Text>
                  <Text style={{fontSize: 15}}><Text style={{fontWeight: 'bold'}}>Street: </Text>{this.props.item.address.street}, {this.props.item.address.buildingNumber},{this.props.item.address.additionalAddressLine}</Text>
                  <Text style={{fontSize: 15}}><Text style={{fontWeight: 'bold'}}>PostCode: </Text>{this.props.item.address.postCode}</Text>
                </View>
            </View>
          :
          <View/>
        }

      </View>
    )
  }
}


const styles = StyleSheet.create({
    infoText:{
      fontSize: 15,
      margin: 2,
    },
      content: {
        margin: 30,
        alignSelf: 'center',
        justifyContent: 'center',
      },
      text:{
        marginTop: 'auto',
        marginBottom: 'auto',
        fontSize: 10,
        color: 'black',
        textAlign: 'center',
    },
    dogPic:{
      height: 100,
      width: 100,
      resizeMode: 'contain',
      aspectRatio: 1, 
    },
});
