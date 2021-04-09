import * as React from 'react';
import { View, StyleSheet,Text,TextInput,Dimensions,TouchableOpacity } from 'react-native';

const {width, height} = Dimensions.get("screen")


export default class DogListItem extends React.Component {
  render(){
    return(
        <TouchableOpacity  style={styles.content}>
              <Text style={styles.text}>id: {this.props.id}</Text>
        </TouchableOpacity>
  )
  }
}


const styles = StyleSheet.create({
    content: {
        height: 200,
        backgroundColor: 'red',
        alignSelf: 'center',
      },
      text:{
        marginTop: 'auto',
        marginBottom: 'auto',
        fontSize: 10,
        color: 'black',
        textAlign: 'center',
    }
});
