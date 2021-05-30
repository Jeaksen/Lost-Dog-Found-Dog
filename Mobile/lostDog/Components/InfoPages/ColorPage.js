import * as React from 'react';
import { View, StyleSheet,Text,TextInput,Dimensions,TouchableOpacity,Image,Keyboard } from 'react-native';
import SkipIcon from '../../Assets/skip.png';

const {width, height} = Dimensions.get("screen")


export default class ColorPage extends React.Component {

    state={
        KeyboardIsOpen: false,
    }
    goToNext=()=>{
        this.props.ParentRef.moveToNext();
    }

  render(){
    return(
        <View style={styles.content}>
          <Text style={styles.Title}>Step 7/7 - Color</Text>
            <View style={styles.grid3x3}>
              <View>
              <TouchableOpacity style={[styles.Button,{backgroundColor: '#444444'}]} onPress={() => this.goToNext()}>
                    <Text style={styles.ButtonText}>Black</Text>
                </TouchableOpacity>

                <TouchableOpacity style={[styles.Button,{backgroundColor: 'brown'}]} onPress={() => this.goToNext()}>
                    <Text style={styles.ButtonText}>Brown</Text>
                </TouchableOpacity>

                <TouchableOpacity style={[styles.Button,{backgroundColor: '#dc5019'}]} onPress={() => this.goToNext()}>
                    <Text style={styles.ButtonText}>Red</Text>
                </TouchableOpacity>
              </View>
              <View>
                <TouchableOpacity style={[styles.Button,{backgroundColor: '#e4e4e4'}]} onPress={() => this.goToNext()}>
                    <Text style={[styles.ButtonText,{color: 'black'}]}>White</Text>
                </TouchableOpacity>

                <TouchableOpacity style={[styles.Button,{backgroundColor: 'gold'}]} onPress={() => this.goToNext()}>
                    <Text style={[styles.ButtonText,{color: 'black'}]}>Gold</Text>
                </TouchableOpacity>

                <TouchableOpacity style={[styles.Button,{backgroundColor: '#ffd96c'}]} onPress={() => this.goToNext()}>
                    <Text style={[styles.ButtonText,{color: 'black'}]}>Yellow</Text>
                </TouchableOpacity>

              </View>
              <View>
                <TouchableOpacity style={[styles.Button,{backgroundColor: 'grey'}]} onPress={() => this.goToNext()}>
                    <Text style={styles.ButtonText}>Grey</Text>
                </TouchableOpacity>

                <TouchableOpacity style={[styles.Button,{backgroundColor: '#ccaf8f'}]} onPress={() => this.goToNext()}>
                    <Text style={styles.ButtonText}>Cream</Text>
                </TouchableOpacity>

                <TouchableOpacity style={[styles.Button,{backgroundColor: '#326b8c'}]} onPress={() => this.goToNext()}>
                    <Text style={styles.ButtonText}>Blue</Text>
                </TouchableOpacity>
              </View>
          </View>
        
        </View>
  )
  }
}


const styles = StyleSheet.create({
    content: {
        height: '100%',
        alignSelf: 'center',
        marginVertical: 'auto',
    },
    grid3x3:{
        flexDirection: 'row',
        flex: 1,
        height: 150, 
        justifyContent: 'center',
        alignItems: 'center',
    },
    Title:{
        fontSize: 20,
        marginTop: 10,
        alignSelf: 'center',
        color: '#99481f',
    },
    Button:{
        width: width*0.2, 
        height: height*0.1, 
        marginBottom:5, 
        marginLeft:5, 
        backgroundColor: '#feb26d',
        borderRadius: 15,
    },
    ButtonText:{
        marginTop: 'auto',
        marginBottom: 'auto',
        fontSize: 13,
        color: 'white',
        textAlign: 'center',
    },
    ButtonLongText:{
        marginTop: 'auto',
        marginBottom: 'auto',
        fontSize: 15,
        color: 'white',
        textAlign: 'center',
        width: '75%',
    },
    ButtonLong:{
        backgroundColor: '#feb26d',
        width: width*0.5,
        height: height*0.06,
        margin: 20,
        marginLeft: 'auto',
        marginRight: 'auto',
        flexDirection: 'row',
        alignContent: 'center',
        borderRadius: 15,
        width: '75%',
    },
    ButtonIcon:{
        width: 35,
        height:35,
        alignSelf: 'center',
        marginRight: 5,
    },
    inputtext: {
        fontSize: 16,
        height: 30,
        width: width*0.6,
        borderColor: '#000000',
        borderWidth: 1,
        borderRadius: 5,
        paddingLeft: 5,
        marginVertical: 10,
        alignSelf: 'center',
      },
});
