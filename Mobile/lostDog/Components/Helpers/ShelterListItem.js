import * as React from 'react';
import { View, StyleSheet,Text,TextInput,Dimensions,TouchableOpacity,Image } from 'react-native';
import ShelterIcon from '../../Assets/animal-shelter.png'

const {width, height} = Dimensions.get("screen")


export default class DogListItem extends React.Component {

  render(){
    //console.log(this.props.item)
    return(
        <TouchableOpacity  style={styles.content}  onPress={() => this.props.shelterSelected(this.props.item)}>
          <View style={{marginHorizontal: 5}}>
              {/*Picture*/
                <Image source={ShelterIcon} style={styles.dogPic}/> 
              }
          </View>
          <View style={{marginVertical: 10, marginHorizontal: 10}}>
              <Text style={{fontSize: 20, fontWeight: 'bold'}}>{this.props.item.name}</Text>
              <Text style={{fontSize: 20}}>city: {this.props.item.address.city}</Text>
              <Text style={{fontSize: 20}}>phone: {this.props.item.phoneNumber}</Text>
          </View>
        </TouchableOpacity>
  )
  }

}


const styles = StyleSheet.create({
    content: {
        alignSelf: 'center',
        flex: 1,
        flexDirection: 'row',
      },
      text:{
        marginTop: 'auto',
        marginBottom: 'auto',
        fontSize: 10,
        color: 'black',
        textAlign: 'center',
    },
    dogPic:{
      height: 140,
      width: 140,
      resizeMode: 'contain',
      aspectRatio: 0.8, 
    },
});
