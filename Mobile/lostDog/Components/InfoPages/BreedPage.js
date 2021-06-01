import * as React from 'react';
import { View, StyleSheet,Text,TextInput,Dimensions,TouchableOpacity,Image,Keyboard } from 'react-native';
import SkipIcon from '../../Assets/skip.png';

const {width, height} = Dimensions.get("screen")


export default class LocationPage extends React.Component {

    state={
        KeyboardIsOpen: false,
        OtherBreed: "",
    }

    goToNext=()=>{
      this.props.ParentRef.moveToNext();
    }
    
    accept=(data)=>{
      this.props.ParentRef.setBreed(data)
      this.goToNext()
    }
    save=(data)=>{

    }

  componentDidMount() {
    this.keyboardDidShowListener = Keyboard.addListener('keyboardDidShow', this._keyboardDidShow);
    this.keyboardDidHideListener = Keyboard.addListener('keyboardDidHide', this._keyboardDidHide);
  }
  componentWillUnmount () {
    this.keyboardDidShowListener.remove();
    this.keyboardDidHideListener.remove();
  }
  /*
    componentWillMount () {
        this.keyboardDidShowListener = Keyboard.addListener('keyboardDidShow', this._keyboardDidShow);
        this.keyboardDidHideListener = Keyboard.addListener('keyboardDidHide', this._keyboardDidHide);
      }
      componentWillUnmount () {
        this.keyboardDidShowListener.remove();
        this.keyboardDidHideListener.remove();
      }
      */
      _keyboardDidShow = () =>{
        //alert('Keyboard Shown');
        this.setState({KeyboardIsOpen: true});
      }
        
      _keyboardDidHide = () =>{
        //alert('Keyboard Hidden');
        this.setState({KeyboardIsOpen: false})
      }
  render(){
    return(
        <View style={styles.content}>
          <Text style={styles.Title}>Step 4/7 - Breed</Text>
            {
              !this.state.KeyboardIsOpen?
            <View style={styles.grid3x3}>
              <View>
                <TouchableOpacity style={styles.Button} onPress={() => this.accept("Labrador Retriever")}>
                    <Text style={styles.ButtonText}>Labrador Retriever	</Text>
                </TouchableOpacity>

                <TouchableOpacity style={styles.Button} onPress={() => this.accept("Yorkshire Terrier")}>
                    <Text style={styles.ButtonText}>Yorkshire Terrier</Text>
                </TouchableOpacity>

                <TouchableOpacity style={styles.Button} onPress={() => this.accept("German Shepherd")}>
                    <Text style={styles.ButtonText}>German Shepherd</Text>
                </TouchableOpacity>
              </View>
              <View>
                <TouchableOpacity style={styles.Button} onPress={() => this.accept("Golden Retriever")}>
                    <Text style={styles.ButtonText}>Golden Retriever</Text>
                </TouchableOpacity>

                <TouchableOpacity style={styles.Button} onPress={() => this.accept("Miniature Schnauzer")}>
                    <Text style={styles.ButtonText}>Miniature Schnauzer</Text>
                </TouchableOpacity>

                <TouchableOpacity style={styles.Button} onPress={() => this.accept("Dachshund")}>
                    <Text style={styles.ButtonText}>Dachshund</Text>
                </TouchableOpacity>

              </View>
              <View>
                <TouchableOpacity style={styles.Button} onPress={() => this.accept("Boxer")}>
                    <Text style={styles.ButtonText}>Boxer</Text>
                </TouchableOpacity>

                <TouchableOpacity style={styles.Button} onPress={() => this.accept("Poodle")}>
                    <Text style={styles.ButtonText}>Poodle</Text>
                </TouchableOpacity>

                <TouchableOpacity style={styles.Button} onPress={() => this.goToNext()}>
                    <Text style={styles.ButtonText}>Skip</Text>
                </TouchableOpacity>
              </View>
          </View>
            :
            <View></View>
            }
          
          <TextInput style={styles.inputtext} placeholder="Other ..." onChangeText={(x) => this.setState({OtherBreed: x})}/>

          {
              !this.state.KeyboardIsOpen?
              <View></View>:
              <View>
                    <TouchableOpacity style={styles.ButtonLong} onPress={() => this.accept(this.state.OtherBreed)}>
                        <Image style={[styles.ButtonIcon, {marginLeft: '5%'}]} source={SkipIcon} />
                        <Text style={styles.ButtonLongText}>Continue</Text>
                    </TouchableOpacity>
              </View>
          }
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
