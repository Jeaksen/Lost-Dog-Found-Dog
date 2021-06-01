import * as React from 'react';
import { View, StyleSheet,Text,TextInput,Dimensions,TouchableOpacity,Image,Keyboard } from 'react-native';
import found from '../Assets/found.png'
import SkipIcon from '../Assets/skip.png';

const {width, height} = Dimensions.get("screen")


export default class CommentPage extends React.Component {

  state={
    KeyboardIsOpen: false,
    TextInputH: 0.2*height,
    message: "",

    BackendState: "waiting",
    itemdata: null,
    itemFilterInfo: null,
  }
  constructor(props)
  {
    super(props);
  }

  componentDidMount(){
    this.keyboardDidShowListener = Keyboard.addListener('keyboardDidShow', this._keyboardDidShow);
    this.keyboardDidHideListener = Keyboard.addListener('keyboardDidHide', this._keyboardDidHide);
    this.setState({itemdata: this.props.item.data})
    this.setState({itemFilterInfo: this.props.item.filterInfo})
  }
  componentWillUnmount(){
    this.keyboardDidShowListener.remove();
    this.keyboardDidHideListener.remove();
  }

  _keyboardDidShow=()=>{
    this.setState({KeyboardIsOpen: true});
    this.setState({TextInputH: 0.4*height});
  }
        
  _keyboardDidHide=()=>{
    this.setState({KeyboardIsOpen: false})
    this.setState({TextInputH: 0.2*height});
  }
  toUri=(picture)=>{
    return "data:" + picture.fileType+";base64,"+picture.data
  }


  send=()=>{
    const photo = {
      uri: this.state.itemFilterInfo.picture,
      type: "image/jpeg",
      name: "photo.jpg"
    };
    const comment=
    {
      text: this.state.message,
      location: 
      {
        city: this.state.itemFilterInfo.locationCity,
        district: this.state.itemFilterInfo.locationDistrict,
      }
    }

    const data = new FormData();
    data.append('comment', JSON.stringify(comment)); 
    data.append('picture', photo);  

    this.props.Navi.RunOnBackend("addComment",{dogId: this.state.itemdata.id, data: data}).then((responseData)=>{
      console.log("Success")
      console.log(responseData)
      this.setState({BackendState: "success"})
    }).catch((x)=>
        {
        console.log("add comment Error" + (x))
        this.setState({BackendState: "error"})
        }
    )
  }

  back=()=>{
    this.props.Navi.swtichPage(6);
  }

  render(){
    if(this.state.itemdata==null)return(<View/>)
    return(
        <View style={styles.content}>
            {
              !this.state.KeyboardIsOpen?
            <View>
              <View style={[{flexDirection: 'row', width: 0.8*width, margin: 20}]}>
                <Image source={found} style={[styles.Icon,{width: 100, height:100}]}/>
                <Text  style={[{ width: '70%', fontSize: 25, fontWeight: 'bold', textAlignVertical: 'center', color: '#99481f'}]}>Leave a comment for the owner of this dog to see.</Text>
              </View>
              <View>
                <View style={[{flexDirection: 'row',height: 0.2*height, width: 0.8*width, margin: 20}]}>
                <View>
                    <Image source={{uri: this.toUri(this.state.itemdata.picture)}} style={[styles.Icon,{width: 0.3*width, height:0.2*height, marginHorizontal: 0.05*width, borderRadius: 500}]}/>
                    <Text style={styles.photoTitle}></Text>
                  </View>
                  <View>
                    <Image source={{uri: this.state.itemFilterInfo.picture}} style={[styles.Icon,{width: 0.3*width, height:0.2*height, marginHorizontal: 0.05*width, borderRadius: 500}]}/>
                    <Text style={styles.photoTitle}></Text>
                  </View>
                </View>
              </View>
            </View>
            :
            <View/>
            }
            {
              this.state.BackendState=="waiting"?
              <TextInput multiline={true} style={[styles.inputtext,{height: this.state.TextInputH}]} placeholder="Place additional information here ..." onChangeText={(x) => this.setState({message: x})}/>
              :<View/>
            }

            {
              this.state.BackendState=="waiting"?
              <TouchableOpacity style={styles.Button} onPress={() => this.send()}>
                    <Text style={styles.ButtonText} >Send</Text>
                    <Image style={[styles.ButtonIcon, {marginRight: '5%'}]} source={SkipIcon} />
              </TouchableOpacity>:
              this.state.BackendState=="success"?
              <View>
                <Text style={{fontSize: 20, alignSelf: 'center', margin: 10}} >Data was sent successfully</Text>
                <TouchableOpacity style={styles.Button} onPress={() => this.back()}>
                      <Text style={styles.ButtonText} >Back</Text>
                      <Image style={[styles.ButtonIcon, {marginRight: '5%'}]} source={SkipIcon} />
                </TouchableOpacity>
              </View>:
              <View>
              <Text style={{fontSize: 20, alignSelf: 'center', margin: 10}} >there was a problem while connecting to the server</Text>
              <TouchableOpacity style={styles.Button} onPress={() => this.back()}>
                    <Text style={styles.ButtonText} >Back</Text>
                    <Image style={[styles.ButtonIcon, {marginRight: '5%'}]} source={SkipIcon} />
              </TouchableOpacity>
              <TouchableOpacity style={styles.Button} onPress={() => this.setState({BackendState: "waiting"})}>
                    <Text style={styles.ButtonText} >Try again</Text>
                    <Image style={[styles.ButtonIcon, {marginRight: '5%'}]} source={SkipIcon} />
              </TouchableOpacity>
            </View>
            }
        </View>
  )
  }
}


const styles = StyleSheet.create({
    photoTitle:{
      alignSelf: 'center',
      fontSize: 15,
      color: '#99481f',
    },
    content: {
        height: '100%',
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
      fontSize: 25,
      color: 'white',
      textAlign: 'center',
      width: '75%',
  },
  ButtonIcon:{
      width: 35,
      height:35,
      alignSelf: 'center',
  },
    inputtext: {
        fontSize: 16,
        height: 0.2*height,
        width: width*0.6,
        borderColor: '#000000',
        borderWidth: 1,
        borderRadius: 5,
        paddingLeft: 5,
        marginVertical: 10,
        alignSelf: 'center',
      },
});
