import * as React from 'react';
import { View, StyleSheet,Text,TextInput,Dimensions,TouchableOpacity } from 'react-native';

const {width, height} = Dimensions.get("screen")


export default class LoadingPage extends React.Component {
  render(){
    return(
        <View style={styles.content}>
            <Text style={styles.logintext} >Loading ... </Text>
        </View>
  )
  }
}


const styles = StyleSheet.create({
  content: {
    position: 'absolute',
    height: '100%',
    width: '100%',
    borderRadius: 100,
    marginHorizontal: 30,
    alignSelf: 'center',
    justifyContent: 'center',
    marginVertical: 'auto',
  },
    logintext:{
    marginTop: 'auto',
    marginBottom: 'auto',
    fontSize: 25,
    textAlign: 'center',
    backgroundColor: 'white',
    height: 0.7*height,
    borderRadius: 100,
    textAlignVertical: 'center'
}
});
