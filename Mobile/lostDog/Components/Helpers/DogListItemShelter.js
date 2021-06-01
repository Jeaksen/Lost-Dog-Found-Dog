import * as React from 'react';
import { View, StyleSheet,Text,TextInput,Dimensions,TouchableOpacity,Image } from 'react-native';

const {width, height} = Dimensions.get("screen")


export default class DogListItemShelter extends React.Component {


  toUri = (picture) =>{
    return "data:" + picture.fileType+";base64,"+picture.data
  }
  statusBar=(isFound)=>{
    var info="IN SHELTER";
    var color="orange";
    if (isFound==false)
    {
      info="LOST";
      color="red";
    }
    return(
      <View style={{transform: [{ translateX: 10},{translateY:-20 }],marginHorizontal:20 ,backgroundColor: color, borderRadius: 20, height: 30, justifyContent: 'center'}}>
        <View style={{alignSelf: 'center', padding: 12}}>
          <Text style={{color: 'white', fontWeight: 'bold'}}>{info}</Text>
        </View>
      </View>
    )
  }


  render(){
    console.log(this.props.item)
    return(
        <TouchableOpacity  style={styles.content} onPress={() => this.props.dogSelected(this.props.item)}>
          <View style={{marginHorizontal: 5}}>
              {/*Picture*/
                this.props.item.picture.data!=null? 
                <Image source={{uri: this.toUri(this.props.item.picture)}} style={styles.dogPic}/> 
                : <View/>
              }
              {/*status*/}
              {this.statusBar(this.props.item.isFound)}
          </View>
          <View style={{marginVertical: 10, marginHorizontal: 10}}>
              <Text style={{fontSize: 20, fontWeight: 'bold'}}>{this.props.item.name}</Text>
              <Text style={{fontSize: 20}}>age: {this.props.item.age}</Text>
              <Text style={{fontSize: 20}}>breed: {this.props.item.breed}</Text>
          </View>
          <View style={{marginVertical: 10, marginHorizontal: 10}}>
              <Text style={{fontSize: 20, fontWeight: 'bold'}}> </Text>
              <Text style={{fontSize: 20}}> </Text>
              <Text style={{fontSize: 20}}> </Text>
              <Text style={{fontSize: 20}}> </Text>
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
      aspectRatio: 0.7, 
      borderRadius: 600,
    },
    statusBas:{

    }
});
