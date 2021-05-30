import * as React from 'react';
import { View, StyleSheet,Text,TextInput,Dimensions,TouchableOpacity,Image } from 'react-native';
import smallDog from '../../Assets/extraSmallDog.png';
import NormalDog from '../../Assets/smallDog.png';
import BigDog from '../../Assets/NormalDog.png';

const {width, height} = Dimensions.get("screen")


export default class SizePage extends React.Component {

    goToNext=()=>{
        this.props.ParentRef.moveToNext();
    }

  render(){
    return(
        <View style={styles.content}>
          <Text style={styles.Title}>Step 6/7 - Size</Text>
          <View>
            <TouchableOpacity style={styles.Button} onPress={() => this.goToNext()}>
                <Image style={[styles.ButtonIcon,{marginLeft: '5%'}]} source={smallDog} />
                <Text style={styles.ButtonText} >Small</Text>
            </TouchableOpacity>

            <TouchableOpacity style={styles.Button} onPress={() => this.goToNext()}>
                <Image style={[styles.ButtonIcon, {marginLeft: '5%'}]} source={NormalDog} />
                <Text style={styles.ButtonText} >Medium</Text>
            </TouchableOpacity>

            <TouchableOpacity style={styles.Button} onPress={() => this.goToNext()}>
                <Image style={[styles.ButtonIcon, {marginLeft: '5%'}]} source={BigDog} />
                <Text style={styles.ButtonText} >Big</Text>
            </TouchableOpacity>
          </View>
        </View>
  )
  }
}


const styles = StyleSheet.create({
    content: {
        height: '100%',
        margin: 50,
        alignSelf: 'center',
        marginVertical: 'auto',
    },
    Title:{
        fontSize: 20,
        marginTop: 10,
        alignSelf: 'center',
        color: '#99481f',
    },
    Button:{
        backgroundColor: '#feb26d',
        width: width*0.5,
        height: height*0.06,
        margin: 20,
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
        width: '75%',
    },
    ButtonIcon:{
        width: 35,
        height:35,
        alignSelf: 'center',
    }
});
